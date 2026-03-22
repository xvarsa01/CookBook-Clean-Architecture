using CookBook.Clean.Application.Filters;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Specifications.Recipe;

public class RecipesBySpecification(RecipeFilter filter, PagingOptions? pagingOptions) : ISpecification<RecipeEntity, RecipeEntity>
{
    
    public IQueryable<RecipeEntity> UseFilter(IQueryable<RecipeEntity> queryable)
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
            var minTimeStr = filter.MinimalDuration.Value.ToString(@"hh\:mm\:ss");
            queryable = queryable.Where(r => string.Compare(r.Duration.ToString(), minTimeStr) >= 0);       // workaround for sqlite
        }

        if (filter.MaximalDuration.HasValue)
        {
            var maxTimeStr = filter.MaximalDuration.Value.ToString(@"hh\:mm\:ss");
            queryable = queryable.Where(r => string.Compare(r.Duration.ToString(), maxTimeStr) <= 0);
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
                ? queryable.OrderBy(r => r.Duration.ToString())                 // .ToString() is workaround for sqlite
                : queryable.OrderByDescending(r => r.Duration.ToString()),
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
