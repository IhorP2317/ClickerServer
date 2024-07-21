using AutoMapper;
using Clicker.BL.Abstractions;
using Clicker.DAL.Models;
using Clicker.Domain.Abstractions;
using Clicker.Domain.Dto.Task;
using Clicker.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Clicker.BL.Implementations;

public class UserOfferTaskService:IUserOfferTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserOfferSubscriptionTaskRepository _userTaskRepository;
    private readonly IBaseEntityRepository<User> _userRepository;
    private readonly IMapper _mapper;
  

    public UserOfferTaskService(IUnitOfWork unitOfWork, IUserOfferSubscriptionTaskRepository userTaskRepository, IBaseEntityRepository<User>  userRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userTaskRepository = userTaskRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<ICollection<UserOfferSubscriptionTaskResponseDto>> GetAllTasksByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {

        if (!await _userRepository.IsExistAsync(id, cancellationToken))
            throw new ApiException($"There are not user with id {id}!", StatusCodes.Status404NotFound);
        var userTasks = await _userTaskRepository.GetByUserIdAsync(id, cancellationToken);
        return _mapper.Map<ICollection<UserOfferSubscriptionTaskResponseDto>>(userTasks);
    }

    public async Task<UserOfferSubscriptionTaskResponseDto> GetTaskByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userTask = await _userTaskRepository.GetQueryable()
            .AsNoTracking()
            .Include(uc => uc.OfferTask)
            .FirstOrDefaultAsync(uc => uc.UserId == id, cancellationToken);
        return _mapper.Map<UserOfferSubscriptionTaskResponseDto>(userTask);
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
        var userTaskToAdd = new UserOfferSubscriptionTask
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

    public Task<bool> IsUserSubscribedAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}