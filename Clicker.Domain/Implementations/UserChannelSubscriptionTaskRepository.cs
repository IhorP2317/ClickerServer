using Clicker.DAL.Data;
using Clicker.DAL.Models;
using Clicker.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Clicker.Domain.Implementations;

public class UserChannelSubscriptionTaskRepository:Repository<UserChannelSubscriptionTask>, IUserChannelSubscriptionTaskRepository
{
    public UserChannelSubscriptionTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<UserChannelSubscriptionTask> GetUserTaskAsync(Guid userId, Guid taskId,
        CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking()
            .Include(uc => uc.User)
            .Include(uc => uc.ChannelSubscriptionTask) 
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.TaskId == taskId, cancellationToken);


    public async Task<IEnumerable<UserChannelSubscriptionTask>?> GetByUserIdAsync(Guid id,
        CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().Where(uc => uc.UserId == id).Include(uc => uc.ChannelSubscriptionTask).ToListAsync(cancellationToken);
   

    public async Task<IEnumerable<UserChannelSubscriptionTask>?> GetByTaskIdAsync(Guid id, CancellationToken cancellationToken = default)=>
        await DbSet.AsNoTracking().Where(uc => uc.TaskId == id).ToListAsync(cancellationToken);


    public async Task<UserChannelSubscriptionTask?> FindAsync(Guid userId, Guid taskId,
        CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().FirstOrDefaultAsync(uc => uc.UserId == userId && uc.TaskId == taskId, cancellationToken);

    public async Task<bool> IsExistAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().AnyAsync(uc => uc.UserId == userId && uc.TaskId == taskId, cancellationToken);

}