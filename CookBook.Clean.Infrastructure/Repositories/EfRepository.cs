using CookBook.Clean.Core;
using CookBook.Clean.UseCases;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.Infrastructure;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly DbContext _dbContext;

    public EfRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }
    
    public async Task<List<TEntity>> GetAllAsync()
    {
        return await GetAllAsync(1, 1000);
    }

    public async Task<List<TEntity>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _dbSet
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
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
        if (!await ExistsAsync(entity))
        {
            return null;
        }

        _dbContext.Set<TEntity>().Attach(entity);
        var updatedEntity = _dbContext.Set<TEntity>().Update(entity);
        await SaveChangesAsync();

        return updatedEntity.Entity.Id;
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
