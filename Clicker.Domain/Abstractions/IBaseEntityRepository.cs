using Clicker.DAL.Models;

namespace Clicker.Domain.Abstractions;

public interface IBaseEntityRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    IQueryable<TEntity> GetQueryable();
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}