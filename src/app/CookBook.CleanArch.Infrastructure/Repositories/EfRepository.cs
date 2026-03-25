using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class EfRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : AggregateRootBase<TId>
    where TId : StronglyTypedId
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly DbContext _dbContext;

    public EfRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    // public async Task<IReadOnlyList<TEntity>> GetListBySpecificationAsync(ISpecification<TEntity, TEntity> specification)
    // {
    //     IQueryable<TEntity> queryable = _dbSet.AsQueryable();
    //     IQueryable<TEntity> query = specification.UseFilter(queryable);
    //     return await query.ToListAsync();
    // }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task DeleteAsync(Guid entityId)
    {
        var entity = await GetByIdAsync(entityId);
        if (entity is null)
        {
            return;
        }
        
        _dbContext.Set<TEntity>().Remove(entity);
        await SaveChangesAsync();
    }

    public async Task<Guid> InsertAsync(TEntity aggregate)
    {
        var entityId = _dbSet.Add(aggregate).Entity.Id;
        await SaveChangesAsync();
        return entityId;
    }

    public async Task<Guid?> UpdateAsync(TEntity aggregate)
    {
        // Assume entity is already tracked if it was loaded via this DbContext
        // Only attach if it's not tracked yet.
        var entry = _dbContext.Entry(aggregate);
        if (entry.State == EntityState.Detached)
        {
            // Attach but do NOT immediately mark as Modified; let individual properties/collections be tracked.
            _dbContext.Set<TEntity>().Attach(aggregate);
        }

        await SaveChangesAsync();
        return aggregate.Id;
    }
    
    public async ValueTask<bool> ExistsAsync(TEntity aggregate)
    {
        return aggregate.Id != Guid.Empty && await _dbSet.AnyAsync(e => e.Id == aggregate.Id);
    }

    protected virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
