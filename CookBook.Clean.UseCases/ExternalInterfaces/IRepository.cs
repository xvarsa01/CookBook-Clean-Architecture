using CookBook.Clean.Core;

namespace CookBook.Clean.UseCases.ExternalInterfaces;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllAsync(int pageNumber, int pageSize);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid entityId);
    Task<Guid> InsertAsync(TEntity entity);
    Task<Guid?> UpdateAsync(TEntity entity);
    ValueTask<bool> ExistsAsync(TEntity entity);
}