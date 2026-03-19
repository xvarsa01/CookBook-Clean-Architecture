using CookBook.Clean.Core;

namespace CookBook.Clean.UseCases.Specifications;

public interface ISpecification<TEntity, TOut > where TEntity : IRootEntity
{
    IQueryable<TEntity> UseFilter(IQueryable<TEntity> queryable);
}
