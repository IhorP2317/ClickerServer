using AutoMapper;
using Clicker.BL.Abstractions;
using Clicker.DAL.Models;
using Clicker.Domain.Abstractions;
using Clicker.Domain.Constants;
using Clicker.Domain.Dto.Task;
using Clicker.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Clicker.BL.Implementations;

public class UserChannelTaskService:IUserChannelTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserChannelSubscriptionTaskRepository _userTaskRepository;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBaseEntityRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly TelegramBotClientConstants _telegramBotClientConstants;

    public UserChannelTaskService(IUnitOfWork unitOfWork, IUserChannelSubscriptionTaskRepository userTaskRepository, IBaseEntityRepository<User> userRepository, IMapper mapper, IOptions<TelegramBotClientConstants> telegramBotClientConstantsOption)
    {
        _unitOfWork = unitOfWork;
        _userTaskRepository = userTaskRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _telegramBotClientConstants = telegramBotClientConstantsOption.Value;
        _telegramBotClient = new TelegramBotClient(_telegramBotClientConstants.ApiToken);
    }
    public async Task<ICollection<UserChannelSubscriptionTaskResponseDto>> GetAllTasksByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {

        if (!await _userRepository.IsExistAsync(id, cancellationToken))
            throw new ApiException($"There are not user with id {id}!", StatusCodes.Status404NotFound);
        var userTasks = await _userTaskRepository.GetByUserIdAsync(id, cancellationToken);
        return _mapper.Map<ICollection<UserChannelSubscriptionTaskResponseDto>>(userTasks);
    }

    public async Task<UserChannelSubscriptionTaskResponseDto> GetTaskByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userTask = await _userTaskRepository.GetQueryable()
            .AsNoTracking()
            .Include(uc => uc.ChannelSubscriptionTask)
            .FirstOrDefaultAsync(uc => uc.UserId == id, cancellationToken);
        return _mapper.Map<UserChannelSubscriptionTaskResponseDto>(userTask);
    }

    public async Task UpdateUserTaskSubscriptionAsync(Guid userId, Guid taskId, bool isSubscribed,
        CancellationToken cancellationToken = default)
    {
        var foundedUserTask = await _userTaskRepository.GetUserTaskAsync(userId, taskId, cancellationToken);
        if(foundedUserTask == null)
            throw new ApiException($"This User Task is not found!", StatusCodes.Status404NotFound);
        foundedUserTask.IsCompleted = isSubscribed;
        _userTaskRepository.Update(foundedUserTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateUserTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
    {
        if (await _userTaskRepository.IsExistAsync(userId, taskId, cancellationToken))
            throw new ApiException($"This User Task is already exist!", StatusCodes.Status409Conflict);
        var userTaskToAdd = new UserChannelSubscriptionTask
        {
            UserId = userId,
            TaskId = taskId,
            IsCompleted = false
        };
        await _userTaskRepository.AddAsync(userTaskToAdd, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
    {
        var userChannelTask = await _userTaskRepository.GetUserTaskAsync(userId, taskId, cancellationToken);
        if (userChannelTask == null)
            throw new ApiException($"There are not user channel task with user id {userId} and task id {taskId}!", StatusCodes.Status404NotFound);
        _userTaskRepository.Delete(userChannelTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsUserSubscribedAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
    {
        bool result  = false;
        var userChannelTask = await _userTaskRepository.GetUserTaskAsync(userId, taskId, cancellationToken);
        if (userChannelTask != null)
        {
           
            var channelId = userChannelTask.ChannelSubscriptionTask.ChannelId;
            if (!channelId.StartsWith("@"))
            {
                channelId = "@" + channelId;
            }
            
            if (!int.TryParse(userChannelTask.User.TelegramId, out var telegramId))
            {
                throw new ApiException("Invalid Telegram ID format", StatusCodes.Status400BadRequest);
            }

            var chatMember = await _telegramBotClient.GetChatMemberAsync(channelId, telegramId, cancellationToken);
            result = chatMember.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Member ||
                     chatMember.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Administrator ||
                     chatMember.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Creator;
        }

        return result;
    }

}