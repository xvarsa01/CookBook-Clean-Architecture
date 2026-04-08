using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Queries.Ingredients;

public record GetIngredientListQuery(IngredientFilter Filter, PagingOptions? PagingOptions = null) : IQuery<PagedResult<IngredientListResponse>>;

internal class GetIngredientListQueryHandler (ICookBookDbContext dbContext) : IQueryHandler<GetIngredientListQuery, PagedResult<IngredientListResponse>>
{
    public async Task<Result<PagedResult<IngredientListResponse>>> Handle(GetIngredientListQuery request, CancellationToken cancellationToken)
    {
        var queryable = dbContext.Ingredients.AsQueryable();
        
        queryable = ApplyFilter(request.Filter, queryable);
        
        var totalItemsCount = await queryable.CountAsync(cancellationToken);
        
        queryable = queryable.ApplyPaging(request.PagingOptions);
        
        var items = await queryable
            .Select(i => new IngredientListResponse(i.Id, i.Name, i.ImageUrl))
            .ToListAsync(cancellationToken);
        
        var result = new PagedResult<IngredientListResponse>
        {
            Items = items,
            TotalItemsCount = totalItemsCount,
            PageIndex = request.PagingOptions?.PageIndex ?? 0,
            PageSize = request.PagingOptions?.PageSize ?? items.Count
        };
        
        return Result.Ok(result);
    }

    private static IQueryable<Ingredient> ApplyFilter(IngredientFilter filter, IQueryable<Ingredient> queryable)
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
        return queryable;
    }
}
