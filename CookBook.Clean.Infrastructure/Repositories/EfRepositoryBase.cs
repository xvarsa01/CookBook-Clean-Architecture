using CookBook.Clean.Core;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.Infrastructure.Repositories;

public class EfRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IRootEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly DbContext _dbContext;

    public EfRepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }
    
    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public async Task<IReadOnlyList<TEntity>> GetListBySpecificationAsync(ISpecification<TEntity, TEntity> specification)
    {
        IQueryable<TEntity> queryable = _dbSet.AsQueryable();
        IQueryable<TEntity> query = specification.UseFilter(queryable);
        return await query.ToListAsync();
    }

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

    public async Task<Guid> InsertAsync(TEntity entity)
    {
        var entityId = _dbSet.Add(entity).Entity.Id;
        await SaveChangesAsync();
        return entityId;
    }

    public async Task<Guid?> UpdateAsync(TEntity entity)
    {
        // Assume entity is already tracked if it was loaded via this DbContext
        // Only attach if it's not tracked yet.
        var entry = _dbContext.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            // Attach but do NOT immediately mark as Modified; let individual properties/collections be tracked.
            _dbContext.Set<TEntity>().Attach(entity);
        }

        await SaveChangesAsync();
        return entity.Id;
    }
    
    public async ValueTask<bool> ExistsAsync(TEntity entity)
    {
        return entity.Id != Guid.Empty && await _dbSet.AnyAsync(e => e.Id == entity.Id);
    }

    protected virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
