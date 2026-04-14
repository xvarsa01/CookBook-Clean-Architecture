using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class EfRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : AggregateRootBase<TId>
    where TId : StronglyTypedId
{
    protected readonly DbSet<TEntity> _dbSet;
    private readonly DbContext _dbContext;

    public EfRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _dbSet.SingleOrDefaultAsync(e => e.Id == id);
    }

    public void Delete(TEntity aggregate)
    {
        _dbContext.Set<TEntity>().Remove(aggregate);
    }

    public TId Add(TEntity aggregate)
    {
        var entityId = _dbSet.Add(aggregate).Entity.Id;
        return entityId;
    }
}
