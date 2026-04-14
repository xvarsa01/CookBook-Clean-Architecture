using System.Collections.Concurrent;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class InMemoryRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : AggregateRootBase<TId>
    where TId : StronglyTypedId
{
    protected readonly ConcurrentDictionary<Guid, TEntity> Store = new();

    public Task<List<TEntity>> GetAllAsync()
        => Task.FromResult(Store.Values.ToList());

    public Task<TEntity?> GetByIdAsync(TId id)
    {
        Store.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public void Delete(TEntity aggregate)
    {
        Store.TryRemove(aggregate.Id, out _);
    }

    public TId Add(TEntity aggregate)
    {
        Store[aggregate.Id.Value] = aggregate;
        return aggregate.Id;
    }
}
