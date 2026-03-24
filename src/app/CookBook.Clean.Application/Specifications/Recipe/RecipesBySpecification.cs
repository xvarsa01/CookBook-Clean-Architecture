using CookBook.Clean.Application.Filters;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Specifications.Recipe;

public class RecipesBySpecification(RecipeFilter filter, PagingOptions? pagingOptions) : ISpecification<RecipeBase, RecipeBase>
{
    
    public IQueryable<RecipeBase> UseFilter(IQueryable<RecipeBase> queryable)
    {
        if (!string.IsNullOrEmpty(filter.Name))
        {
            queryable = queryable.Where(r => r.Name.Value.ToLower().Contains(filter.Name.ToLower()));
        }

        if (filter.RecipeType is not null)
        {
            queryable = queryable.Where(r => r.Type == filter.RecipeType);
        }

        if (filter.MinimalDuration.HasValue)
        {
            var min = RecipeDuration.CreateObject(filter.MinimalDuration.Value).Value;
            queryable = queryable.Where(r => r.Duration >= min);
        }

        if (filter.MaximalDuration.HasValue)
        {
            var max = RecipeDuration.CreateObject(filter.MaximalDuration.Value).Value;
            queryable = queryable.Where(r => r.Duration <= max);
        }
        
        queryable = filter.SortParameter switch
        {
            RecipeSortParameter.Name => filter.IsSortAscending 
                ? queryable.OrderBy(r => r.Name)
                : queryable.OrderByDescending(r => r.Name),
            
            RecipeSortParameter.Type => filter.IsSortAscending
                ? queryable.OrderBy(r => r.Type)
                : queryable.OrderByDescending(r => r.Type),
            
            RecipeSortParameter.Duration => filter.IsSortAscending
                ? queryable.OrderBy(r => r.Duration)
                : queryable.OrderByDescending(r => r.Duration),
            
            RecipeSortParameter.CreatedAt => filter.IsSortAscending
                ? queryable.OrderBy(r => r.CreatedAt)
                : queryable.OrderByDescending(r => r.CreatedAt),
            
            RecipeSortParameter.ModifiedAt => filter.IsSortAscending
                ? queryable.OrderBy(r => r.ModifiedAt)
                : queryable.OrderByDescending(r => r.ModifiedAt),
            
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
