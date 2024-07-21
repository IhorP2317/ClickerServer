using Clicker.DAL.Models;

namespace Clicker.Domain.Abstractions;

public interface IUserOfferSubscriptionTaskRepository
{
    Task<IEnumerable<UserOfferSubscriptionTask>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<UserOfferSubscriptionTask> GetUserTaskAsync(Guid userId, Guid taskId,
        CancellationToken cancellationToken = default);
    IQueryable<UserOfferSubscriptionTask> GetQueryable();
    Task<IEnumerable<UserOfferSubscriptionTask>?> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserOfferSubscriptionTask>?> GetByTaskIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserOfferSubscriptionTask?> FindAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(Guid userId, Guid taskId,  CancellationToken cancellationToken = default);
    Task<UserOfferSubscriptionTask> AddAsync(UserOfferSubscriptionTask userTask, CancellationToken cancellationToken = default);
    void Update(UserOfferSubscriptionTask userTask);
    void Delete(UserOfferSubscriptionTask userTask);
}