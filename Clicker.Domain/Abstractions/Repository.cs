using Clicker.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Clicker.Domain.Abstractions;

public abstract class Repository<TEntity> where TEntity :class
{
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbSet = dbContext.Set<TEntity>();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().ToListAsync(cancellationToken);
    public  IQueryable<TEntity> GetQueryable() => DbSet.AsQueryable();

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
       var createdEntity =  await DbSet.AddAsync(entity, cancellationToken);
       return createdEntity.Entity;
    } 
       
    public void Update(TEntity entity) => DbSet.Update(entity);
    public void Delete(TEntity entity) => DbSet.Remove(entity);
}