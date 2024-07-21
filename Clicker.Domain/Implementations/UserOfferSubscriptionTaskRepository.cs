using Clicker.DAL.Data;
using Clicker.DAL.Models;
using Clicker.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Clicker.Domain.Implementations;

public class UserOfferSubscriptionTaskRepository:Repository<UserOfferSubscriptionTask>, IUserOfferSubscriptionTaskRepository
{
    public UserOfferSubscriptionTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserOfferSubscriptionTask> GetUserTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default) =>
            await DbSet.AsNoTracking()
                .Include(uc => uc.User)
                .Include(uc => uc.OfferTask) 
                .SingleOrDefaultAsync(uc => uc.UserId == userId && uc.TaskId == taskId, cancellationToken);

    public async Task<IEnumerable<UserOfferSubscriptionTask>?> GetByUserIdAsync(Guid id,
        CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().Where(uc => uc.UserId == id).ToListAsync(cancellationToken);
   

    public async Task<IEnumerable<UserOfferSubscriptionTask>?> GetByTaskIdAsync(Guid id, CancellationToken cancellationToken = default)=>
        await DbSet.AsNoTracking().Where(uc => uc.TaskId == id).ToListAsync(cancellationToken);


    public async Task<UserOfferSubscriptionTask?> FindAsync(Guid userId, Guid taskId,
        CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().SingleOrDefaultAsync(uc => uc.UserId == userId && uc.TaskId == taskId, cancellationToken);

    public async Task<bool> IsExistAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().AnyAsync(uc => uc.UserId == userId && uc.TaskId == taskId, cancellationToken);
}