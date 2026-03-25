using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Application.ExternalInterfaces;

public interface IRepository<TEntity, TId>
    where TEntity : AggregateRootBase<TId> where TId : StronglyTypedId
{
    IQueryable<TEntity> Query();
    Task<List<TEntity>> GetAllAsync();
    // Task<IReadOnlyList<TEntity>> GetListBySpecificationAsync(ISpecification<TEntity, TEntity> specification);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid entityId);
    Task<Guid> InsertAsync(TEntity entity);
    Task<Guid?> UpdateAsync(TEntity entity);
    ValueTask<bool> ExistsAsync(TEntity entity);
}
