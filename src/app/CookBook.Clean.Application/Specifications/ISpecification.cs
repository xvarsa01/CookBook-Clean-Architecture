using CookBook.Clean.Core;

namespace CookBook.Clean.Application.Specifications;

public interface ISpecification<in TEntity, out TOut > where TEntity : IAggregateRootEntity
{
    IQueryable<TOut> UseFilter(IQueryable<TEntity> queryable);
}
