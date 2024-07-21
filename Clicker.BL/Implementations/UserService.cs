using System.Net;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Clicker.BL.Abstractions;
using Clicker.DAL.Models;
using Clicker.Domain.Abstractions;
using Clicker.Domain.Constants;
using Clicker.Domain.Dto;
using Clicker.Domain.Dto.Task;
using Clicker.Domain.Enums;
using Clicker.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Clicker.BL.Implementations;

public class UserService: IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseEntityRepository<User> _userRepository;
    private readonly HttpClient _httpClient;
    private readonly SecurityHttpClientConstants _securityHttpClientConstants;
    private readonly IMapper _mapper;
    private readonly IUserOfferTaskService _userOfferTaskService;
    private readonly IUserChannelSubscriptionTaskRepository _userChannelRepository;
    private readonly IUserOfferSubscriptionTaskRepository _userOfferRepository;
    private readonly IUserChannelTaskService _userChannelTaskService;

    public UserService(IUnitOfWork unitOfWork, IBaseEntityRepository<User> userRepository, 
        IHttpClientFactory httpClientFactory , IOptions<SecurityHttpClientConstants> securityHttpClientConstants, 
       IMapper mapper, IUserOfferTaskService userOfferTaskService, IUserChannelTaskService userChannelTaskService , IUserOfferSubscriptionTaskRepository userOfferRepository, IUserChannelSubscriptionTaskRepository userChannelRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _securityHttpClientConstants = securityHttpClientConstants.Value;
        _httpClient = httpClientFactory.CreateClient(_securityHttpClientConstants.ClientName);
        _mapper = mapper;
        _userOfferTaskService = userOfferTaskService;
        _userChannelTaskService = userChannelTaskService;
        _userChannelRepository = userChannelRepository;
        _userOfferRepository = userOfferRepository;
    }
    public async Task<UserResponseDto> RegisterUserAsync(UserSignUpDto userSignUpDto, CancellationToken cancellationToken = default)
    {
        if (userSignUpDto.ReferrerId.HasValue)
        {
            var referrer = await _userRepository.GetByIdAsync(userSignUpDto.ReferrerId.Value, cancellationToken);
            if (referrer == null)
            {
                throw new ApiException("Referrer is not found!", StatusCodes.Status404NotFound);
            }
        }
        var jsonContent = JsonSerializer.Serialize(userSignUpDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var url = _httpClient.BaseAddress.ToString() + _securityHttpClientConstants.RegisterEndpoint;
        var httpResponse = await _httpClient.PostAsync(url, content, cancellationToken);
        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            var problemDetails = await HandleHttpResponse<ProblemDetails>(httpResponse, cancellationToken);
            throw new ApiException(problemDetails.Detail, (int)httpResponse.StatusCode);
        }

        var responseBody = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        var securityUserResponseDto = await HandleHttpResponse<UserSecurityResponseDto>(httpResponse, cancellationToken);
        
        var userToAdd = _mapper.Map<User>(securityUserResponseDto);
        userToAdd.Energy = (int)EnergyLimit.DefaultUser;
        var createdUser = await _userRepository.AddAsync(userToAdd, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserResponseDto>(createdUser);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
            throw new ApiException($"There are not user with id {id}!", StatusCodes.Status404NotFound);
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<bool> IsUserExistAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _userRepository.IsExistAsync(id, cancellationToken);
    }

    public async Task ClickAsync(Guid userId, int clickCount = 1, CancellationToken cancellationToken = default)
    {
        var userDto =  await  GetUserByIdAsync(userId, cancellationToken);
        var energyLimit = await CalculateEnergyLimitToRecoverAsync(userId, cancellationToken);
        var user = _mapper.Map<User>(userDto);
        if (user.Energy <= 0)
        {
            throw new ApiException("No energy left to perform clicks.", StatusCodes.Status400BadRequest);
        }
        
        int clicksToPerform = Math.Min(user.Energy, clickCount);
        
        user.Energy -= clicksToPerform;
        user.Balance += clicksToPerform * 0.001m; 

        user.Energy = Math.Min(user.Energy, (int)energyLimit);
        
        _userRepository.Update(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

       
        if (clicksToPerform < clickCount)
        {
            throw new ApiException($"User could only perform {clicksToPerform} out of {clickCount} clicks due to insufficient energy.", StatusCodes.Status400BadRequest);
        }
    }

    public async Task<UserTasksResponseDto> GetUserTasksAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByIdAsync(id, cancellationToken);
        var channelTasks = await _userChannelTaskService.GetAllTasksByUserIdAsync(user.Id, cancellationToken);
        var offerTasks = await _userOfferTaskService.GetAllTasksByUserIdAsync(user.Id, cancellationToken); 


        return new UserTasksResponseDto
        (channelTasks,
            offerTasks
        );
    }

  public async Task AdjustUserEnergyAsync(Guid userId, CancellationToken cancellationToken = default)
{
    var user = await _userRepository.GetByIdAsync(userId, cancellationToken); 
    if (user == null)
    {
        throw new ApiException($"User with id {userId} not found!", StatusCodes.Status404NotFound);
    }

    var (isChannelSubscribed, isOfferSubscribed, userChannelTask, userOfferTask) = await GetSubscriptionStatusAndTasksAsync(user.Id, cancellationToken);

    var newEnergyLimit = CalculateEnergyLimit(isChannelSubscribed, isOfferSubscribed, userChannelTask, userOfferTask);

    if (newEnergyLimit > 0)
    {
        user.Energy = newEnergyLimit; 
        UpdateTaskSubscriptionAsync(isChannelSubscribed, isOfferSubscribed, userChannelTask, userOfferTask, cancellationToken);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

private  void UpdateTaskSubscriptionAsync(bool isChannelSubscribed, bool isOfferSubscribed, UserChannelSubscriptionTask? userChannelTask, UserOfferSubscriptionTask? userOfferTask, CancellationToken cancellationToken = default)
{
    if (userChannelTask != null && isChannelSubscribed && !userChannelTask.IsCompleted)
    {
        userChannelTask.IsCompleted = true;
        _userChannelRepository.Update(userChannelTask); // Assuming _userTaskRepository is your repository for UserChannelSubscriptionTask
    }

    if (userOfferTask != null && isOfferSubscribed && !userOfferTask.IsCompleted)
    {
        userOfferTask.IsCompleted = true;
        _userOfferRepository.Update(userOfferTask); // Assuming _userTaskRepository is your repository for UserOfferSubscriptionTask
    }
}

private int CalculateEnergyLimit(bool isChannelSubscribed, bool isOfferSubscribed, UserChannelSubscriptionTask? userChannelTask, UserOfferSubscriptionTask? userOfferTask)
{
    if (userChannelTask != null && userOfferTask != null && 
        !userChannelTask.IsCompleted && isChannelSubscribed && 
        !userOfferTask.IsCompleted && isOfferSubscribed)
    {
        return (int)EnergyLimit.CompletedTwoTaskUser;
    }
    else if (userChannelTask != null && !userChannelTask.IsCompleted && isChannelSubscribed)
    {
        return (int)EnergyLimit.CompletedOneTaskUser;
    }
    else if (userOfferTask != null && !userOfferTask.IsCompleted && isOfferSubscribed)
    {
        return (int)EnergyLimit.CompletedOneTaskUser;
    }

    return 0; 
}

private async Task<(bool isChannelSubscribed, bool isOfferSubscribed, UserChannelSubscriptionTask? userChannelTask, UserOfferSubscriptionTask? userOfferTask)> GetSubscriptionStatusAndTasksAsync(Guid userId, CancellationToken cancellationToken)
{
    var (isChannelSubscribed, isOfferSubscribed) = await CheckUserSubscriptionStatus(userId, cancellationToken);
    var userChannelTask = await _userChannelRepository.GetQueryable()
            .AsNoTracking()
            .Include(uc => uc.ChannelSubscriptionTask)
            .FirstOrDefaultAsync(uc => uc.UserId == userId, cancellationToken);
    var userOfferTask = await _userOfferRepository.GetQueryable()
        .AsNoTracking()
        .Include(uc => uc.OfferTask)
        .FirstOrDefaultAsync(uc => uc.UserId == userId, cancellationToken);
    return (isChannelSubscribed, isOfferSubscribed, userChannelTask, userOfferTask);
}


    public async Task<EnergyLimit> CalculateEnergyLimitToRecoverAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        
        var subscriptionStatus = await CheckUserSubscriptionStatus(userId, cancellationToken);
        return subscriptionStatus switch
        {
            (true, true) => EnergyLimit.CompletedTwoTaskUser,
            (true, false) => EnergyLimit.CompletedOneTaskUser,
            (false, true) => EnergyLimit.CompletedOneTaskUser,
            _ => EnergyLimit.DefaultUser
        };
    }
    private static async Task<T> HandleHttpResponse<T>(HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default) {
        await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        var securityUserResponseDto = await JsonSerializer.DeserializeAsync<T>(
            stream,
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            },
            cancellationToken
        );

        return securityUserResponseDto;
    }

    public async Task<(bool, bool)> CheckUserSubscriptionStatus(Guid id, CancellationToken cancellationToken = default)
    {
       
        var channelTask = await _userChannelTaskService.GetTaskByUserIdAsync(id, cancellationToken);
        var offerTask = await _userOfferTaskService.GetTaskByUserIdAsync(id, cancellationToken);
        

        
        var isChannelTaskCompleted = channelTask != null ? await _userChannelTaskService.IsUserSubscribedAsync(id, channelTask.TaskId, cancellationToken) : false;
        var isOfferTaskCompleted = offerTask != null ? await _userOfferTaskService.IsUserSubscribedAsync(id, offerTask.TaskId, cancellationToken ) : false;
        
        
        return (isChannelTaskCompleted, isOfferTaskCompleted);
    }

}