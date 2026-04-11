using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Application.ExternalInterfaces;

public interface IRepository<TAggregate, TId>
    where TAggregate : AggregateRootBase<TId> where TId : StronglyTypedId
{
    Task<List<TAggregate>> GetAllAsync();
    Task<TAggregate?> GetByIdAsync(TId id);
    Task DeleteAsync(TId entityId);
    TId Add(TAggregate aggregate);
    ValueTask<bool> ExistsAsync(TAggregate aggregate);
}
