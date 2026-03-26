using System.Collections.Concurrent;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class InMemoryRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : AggregateRootBase<TId>
    where TId : StronglyTypedId
{
    private readonly ConcurrentDictionary<Guid, TEntity> _store = new();

    public IQueryable<TEntity> Query()
    {
        return _store.Values.AsQueryable();
    }

    public Task<List<TEntity>> GetAllAsync()
        => Task.FromResult(_store.Values.ToList());

    public Task<TEntity?> GetByIdAsync(TId id)
    {
        _store.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(TId entityId)
    {
        _store.TryRemove(entityId, out _);
        return Task.CompletedTask;
    }

    public Task<TId> InsertAsync(TEntity aggregate)
    {
        _store[aggregate.Id.Value] = aggregate;
        return Task.FromResult(aggregate.Id);
    }

    public Task<TId?> UpdateAsync(TEntity aggregate)
    {
        if (!_store.ContainsKey(aggregate.Id))
            return Task.FromResult<TId?>(null);

        _store[aggregate.Id] = aggregate;
        return Task.FromResult<TId?>(aggregate.Id);
    }

    public ValueTask<bool> ExistsAsync(TEntity aggregate)
        => new(_store.ContainsKey(aggregate.Id));
}
