using System.Collections.Concurrent;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class InMemoryRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : AggregateRootBase<TId>
    where TId : StronglyTypedId
{
    private readonly ConcurrentDictionary<Guid, TEntity> _store = new();

    public Task<List<TEntity>> GetAllAsync()
        => Task.FromResult(_store.Values.ToList());

    public Task<TEntity?> GetByIdAsync(TId id)
    {
        _store.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public void Delete(TEntity aggregate)
    {
        _store.TryRemove(aggregate.Id, out _);
    }

    public TId Add(TEntity aggregate)
    {
        _store[aggregate.Id.Value] = aggregate;
        return aggregate.Id;
    }
}
