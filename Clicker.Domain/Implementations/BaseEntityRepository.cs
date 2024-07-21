using Clicker.DAL.Data;
using Clicker.DAL.Models;
using Clicker.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Clicker.Domain.Implementations;

public  class BaseEntityRepository<TEntity>: Repository<TEntity>, IBaseEntityRepository<TEntity> where TEntity : BaseEntity
{
   

    public BaseEntityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);


    public async Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().AnyAsync(t => t.Id == id, cancellationToken);

}