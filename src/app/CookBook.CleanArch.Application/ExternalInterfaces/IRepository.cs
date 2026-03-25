using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Application.ExternalInterfaces;

public interface IRepository<TAggregate, TId>
    where TAggregate : AggregateRootBase<TId> where TId : StronglyTypedId
{
    IQueryable<TAggregate> Query();
    Task<List<TAggregate>> GetAllAsync();
    // Task<IReadOnlyList<TEntity>> GetListBySpecificationAsync(ISpecification<TEntity, TEntity> specification);
    Task<TAggregate?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid entityId);
    Task<Guid> InsertAsync(TAggregate aggregate);
    Task<Guid?> UpdateAsync(TAggregate aggregate);
    ValueTask<bool> ExistsAsync(TAggregate aggregate);
}
