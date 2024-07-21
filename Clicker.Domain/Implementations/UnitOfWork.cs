using Clicker.DAL.Data;
using Clicker.Domain.Abstractions;

namespace Clicker.Domain.Implementations;

public class UnitOfWork:IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken);
}