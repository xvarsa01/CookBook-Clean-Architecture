using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.Filters;

namespace CookBook.Clean.UseCases.Specifications.Recipe;

public class RecipesBySpecification(RecipeFilter filter, PagingOptions? pagingOptions) : ISpecification<RecipeEntity, RecipeEntity>
{
    
    public IQueryable<RecipeEntity> UseFilter(IQueryable<RecipeEntity> queryable)
    {
        if (!string.IsNullOrEmpty(filter.Name))
        {
            queryable = queryable.Where(r => r.Name.Contains(filter.Name));
        }

        if (filter.RecipeType is not null)
        {
            queryable = queryable.Where(r => r.Type == filter.RecipeType);
        }

        if (filter.MinimalDuration is not null)
        {
            queryable = queryable.Where(r => r.Duration >= filter.MinimalDuration);
        }

        if (filter.MaximalDuration is not null)
        {
            queryable = queryable.Where(r => r.Duration <= filter.MaximalDuration);
        }
        
        queryable = filter.SortParameterName?.ToLower() switch
        {
            "name" => filter.IsSortAscending
                ? queryable.OrderBy(r => r.Name)
                : queryable.OrderByDescending(r => r.Name),
            "type" => filter.IsSortAscending
                ? queryable.OrderBy(r => r.Type)
                : queryable.OrderByDescending(r => r.Type),
            "duration" => filter.IsSortAscending
                ? queryable.OrderBy(r => r.Duration)
                : queryable.OrderByDescending(r => r.Duration),
            _ => queryable.OrderBy(r => r.Name)
        };

        if (pagingOptions is null)
        {
            return queryable;
        }

        return queryable
            .Skip(pagingOptions.PageSize * pagingOptions.PageIndex)
            .Take(pagingOptions.PageSize);
    }
}
