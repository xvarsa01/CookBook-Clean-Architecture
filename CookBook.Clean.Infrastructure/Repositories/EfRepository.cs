using CookBook.Clean.Core;
using CookBook.Clean.UseCases;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.Infrastructure;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(DbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }
    
    public Task<List<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> GetAllAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task DeleteAsync(Guid entityId)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> InsertAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<Guid?> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> ExistsAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
