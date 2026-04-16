using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Recipes.Queries;

public record GetRecipeListQuery(RecipeFilter Filter, PagingOptions? PagingOptions = null) : IQuery<PagedResult<RecipeListResponse>>;

internal class GetRecipeListQueryHandler(ICookBookDbContext dbContext) : IQueryHandler<GetRecipeListQuery, PagedResult<RecipeListResponse>>
{
    public async Task<Result<PagedResult<RecipeListResponse>>> Handle(GetRecipeListQuery request, CancellationToken cancellationToken)
    {
        var queryable = dbContext.Recipes.AsQueryable();
        
        queryable = ApplyFilter(request.Filter, queryable);
        
        var totalItemsCount = await queryable.CountAsync(cancellationToken);
        
        queryable = queryable.ApplyPaging(request.PagingOptions);
        
        var items = await queryable.Select(i => new RecipeListResponse(i.Id, i.Name, i.ImageUrl, i.Type)).ToListAsync(cancellationToken);
        
        var result = new PagedResult<RecipeListResponse>
        {
            Items = items,
            TotalItemsCount = totalItemsCount,
            PageIndex = request.PagingOptions?.PageIndex ?? 0,
            PageSize = request.PagingOptions?.PageSize ?? items.Count
        };
        return Result.Success(result);
    }

    private static IQueryable<Recipe> ApplyFilter(RecipeFilter filter, IQueryable<Recipe> queryable)
    {
        if (!string.IsNullOrEmpty(filter.Name))
        {
            var nameFilter = $"%{filter.Name}%";
            queryable = queryable.Where(r => EF.Functions.Like((string)r.Name, nameFilter));
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
                ? queryable.OrderBy(r => ((string)r.Name).ToUpper()).ThenBy(r => r.Name)
                : queryable.OrderByDescending(r => ((string)r.Name).ToUpper()).ThenByDescending(r => r.Name),
            
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
            
            _ => queryable.OrderBy(r => ((string)r.Name).ToUpper()).ThenBy(r => r.Name)
        };
        return queryable;
    }
}
