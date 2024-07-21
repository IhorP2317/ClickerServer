using Clicker.DAL.Models;

namespace Clicker.Domain.Abstractions;

public interface IUserChannelSubscriptionTaskRepository
{
    Task<UserChannelSubscriptionTask> GetUserTaskAsync(Guid userId, Guid taskId,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<UserChannelSubscriptionTask>> GetAllAsync(CancellationToken cancellationToken = default);
    IQueryable<UserChannelSubscriptionTask> GetQueryable();
    Task<IEnumerable<UserChannelSubscriptionTask>?> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserChannelSubscriptionTask>?> GetByTaskIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserChannelSubscriptionTask?> FindAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(Guid userId, Guid taskId,  CancellationToken cancellationToken = default);
    Task<UserChannelSubscriptionTask> AddAsync(UserChannelSubscriptionTask userTask, CancellationToken cancellationToken = default);
    void Update(UserChannelSubscriptionTask userTask);
    void Delete(UserChannelSubscriptionTask userTask);
}