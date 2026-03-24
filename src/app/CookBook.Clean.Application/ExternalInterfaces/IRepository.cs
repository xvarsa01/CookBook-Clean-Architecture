using CookBook.Clean.Application.Specifications;
using CookBook.Clean.Core;

namespace CookBook.Clean.Application.ExternalInterfaces;

public interface IRepository<TEntity>
    where TEntity : class, IAggregateRootEntity
{
    IQueryable<TEntity> Query();
    Task<List<TEntity>> GetAllAsync();
    Task<IReadOnlyList<TEntity>> GetListBySpecificationAsync(ISpecification<TEntity, TEntity> specification);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid entityId);
    Task<Guid> InsertAsync(TEntity entity);
    Task<Guid?> UpdateAsync(TEntity entity);
    ValueTask<bool> ExistsAsync(TEntity entity);
}
