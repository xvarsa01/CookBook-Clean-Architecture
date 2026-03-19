using CookBook.Clean.Core;
using CookBook.Clean.UseCases.Specifications;

namespace CookBook.Clean.UseCases.ExternalInterfaces;

public interface IRepository<TEntity>
    where TEntity : class, IRootEntity
{
    Task<List<TEntity>> GetAllAsync();
    Task<IReadOnlyList<TEntity>> GetListBySpecificationAsync(ISpecification<TEntity, TEntity> specification);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid entityId);
    Task<Guid> InsertAsync(TEntity entity);
    Task<Guid?> UpdateAsync(TEntity entity);
    ValueTask<bool> ExistsAsync(TEntity entity);
}
