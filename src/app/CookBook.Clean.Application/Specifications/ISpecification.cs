using CookBook.Clean.Core;
using CookBook.Clean.Core.Shared;

namespace CookBook.Clean.Application.Specifications;

public interface ISpecification<in TEntity, out TOut > where TEntity : AggregateRootBase
{
    IQueryable<TOut> UseFilter(IQueryable<TEntity> queryable);
}
