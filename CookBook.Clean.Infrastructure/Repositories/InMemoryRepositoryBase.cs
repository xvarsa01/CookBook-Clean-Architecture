using System.Collections.Concurrent;
using CookBook.Clean.Core;
using CookBook.Clean.UseCases;
using CookBook.Clean.UseCases.ExternalInterfaces;

namespace CookBook.Clean.Infrastructure.Repositories;

public class InMemoryRepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly ConcurrentDictionary<Guid, TEntity> _store = new();

    public Task<List<TEntity>> GetAllAsync()
        => Task.FromResult(_store.Values.ToList());

    public Task<List<TEntity>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = _store.Values
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<TEntity?> GetByIdAsync(Guid id)
    {
        _store.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(Guid entityId)
    {
        _store.TryRemove(entityId, out _);
        return Task.CompletedTask;
    }

    public Task<Guid> InsertAsync(TEntity entity)
    {
        _store[entity.Id] = entity;
        return Task.FromResult(entity.Id);
    }

    public Task<Guid?> UpdateAsync(TEntity entity)
    {
        if (!_store.ContainsKey(entity.Id))
            return Task.FromResult<Guid?>(null);

        _store[entity.Id] = entity;
        return Task.FromResult<Guid?>(entity.Id);
    }

    public ValueTask<bool> ExistsAsync(TEntity entity)
        => new(_store.ContainsKey(entity.Id));
}
