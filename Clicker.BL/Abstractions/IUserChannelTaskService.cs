using Clicker.Domain.Dto.Task;

namespace Clicker.BL.Abstractions;

public interface IUserChannelTaskService
{
    Task<ICollection<UserChannelSubscriptionTaskResponseDto>> GetAllTasksByUserIdAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task<UserChannelSubscriptionTaskResponseDto> GetTaskByUserIdAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task UpdateUserTaskSubscriptionAsync(Guid userId, Guid taskId, bool isSubscribed,
        CancellationToken cancellationToken = default);

    Task CreateUserTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
    Task DeleteUserTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
    Task<bool> IsUserSubscribedAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
}