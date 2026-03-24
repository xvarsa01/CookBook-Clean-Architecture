using CookBook.Clean.Application.Filters;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Specifications.Ingredient;

public class IngredientsBySpecification(IngredientFilter filter, PagingOptions? pagingOptions) : ISpecification<IngredientBase, IngredientBase>
{
    public IQueryable<IngredientBase> UseFilter(IQueryable<IngredientBase> queryable)
    {
        if (filter.Name is not null)
        {
            queryable = queryable.Where(i => i.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        if (filter.HasDescription is not null)
        {
            queryable = filter.HasDescription.Value
                ? queryable.Where(i => i.Description != null)
                : queryable.Where(i => i.Description == null);
        }

        if (filter.HasImage is not null)
        {
            queryable = filter.HasImage.Value
                ? queryable.Where(i => i.ImageUrl != null)
                : queryable.Where(i => i.ImageUrl == null);
        }
        
        queryable = filter.SortParameter switch
        {
            IngredientsSortParameter.Name => filter.IsSortAscending
                ? queryable.OrderBy(r => r.Name)
                : queryable.OrderByDescending(r => r.Name),
            
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
