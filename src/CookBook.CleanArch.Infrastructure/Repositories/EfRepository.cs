using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class EfRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : AggregateRootBase<TId>
    where TId : StronglyTypedId
{
    protected readonly DbSet<TEntity> DbSet;

    public EfRepository(DbContext dbContext)
    {
        DbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await DbSet.SingleOrDefaultAsync(e => e.Id == id);
    }

    public void Delete(TEntity aggregate)
    {
        DbSet.Remove(aggregate);
    }

    public TId Add(TEntity aggregate)
    {
        var entityId = DbSet.Add(aggregate).Entity.Id;
        return entityId;
    }
}
