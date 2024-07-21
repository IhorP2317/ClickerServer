using Clicker.Domain.Dto.Task;

namespace Clicker.BL.Abstractions;

public interface IUserOfferTaskService
{
    Task<ICollection<UserOfferSubscriptionTaskResponseDto>> GetAllTasksByUserIdAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task<UserOfferSubscriptionTaskResponseDto> GetTaskByUserIdAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task UpdateUserTaskSubscriptionAsync(Guid userId, Guid taskId, bool isSubscribed,
        CancellationToken cancellationToken = default);

    Task CreateUserTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
    Task DeleteUserTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
    Task<bool> IsUserSubscribedAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
}