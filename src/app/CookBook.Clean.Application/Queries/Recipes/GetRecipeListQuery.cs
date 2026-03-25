using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListQuery(RecipeFilter Filter, PagingOptions? PagingOptions = null) : IQuery<List<RecipeGetListResponse>>;

internal class GetRecipeListQueryHandler(IRepository<Recipe, RecipeId> repository) : IQueryHandler<GetRecipeListQuery, List<RecipeGetListResponse>>
{
    public Task<Result<List<RecipeGetListResponse>>> Handle(GetRecipeListQuery request, CancellationToken cancellationToken)
    {
        var queryable = repository.Query();
        
        if (!string.IsNullOrEmpty(request.Filter.Name))
        {
            queryable = queryable.Where(r => r.Name.Value.ToLower().Contains(request.Filter.Name.ToLower()));
        }

        if (request.Filter.RecipeType is not null)
        {
            queryable = queryable.Where(r => r.Type == request.Filter.RecipeType);
        }

        if (request.Filter.MinimalDuration.HasValue)
        {
            var min = RecipeDuration.CreateObject(request.Filter.MinimalDuration.Value).Value;
            queryable = queryable.Where(r => r.Duration >= min);
        }

        if (request.Filter.MaximalDuration.HasValue)
        {
            var max = RecipeDuration.CreateObject(request.Filter.MaximalDuration.Value).Value;
            queryable = queryable.Where(r => r.Duration <= max);
        }
        
        queryable = request.Filter.SortParameter switch
        {
            RecipeSortParameter.Name => request.Filter.IsSortAscending 
                ? queryable.OrderBy(r => r.Name)
                : queryable.OrderByDescending(r => r.Name),
            
            RecipeSortParameter.Type => request.Filter.IsSortAscending
                ? queryable.OrderBy(r => r.Type)
                : queryable.OrderByDescending(r => r.Type),
            
            RecipeSortParameter.Duration => request.Filter.IsSortAscending
                ? queryable.OrderBy(r => r.Duration)
                : queryable.OrderByDescending(r => r.Duration),
            
            RecipeSortParameter.CreatedAt => request.Filter.IsSortAscending
                ? queryable.OrderBy(r => r.CreatedAt)
                : queryable.OrderByDescending(r => r.CreatedAt),
            
            RecipeSortParameter.ModifiedAt => request.Filter.IsSortAscending
                ? queryable.OrderBy(r => r.ModifiedAt)
                : queryable.OrderByDescending(r => r.ModifiedAt),
            
            _ => queryable.OrderBy(r => r.Name)
        };
        
        if (request.PagingOptions is not null)
        {
            queryable = queryable
                .Skip(request.PagingOptions.PageSize * request.PagingOptions.PageIndex)
                .Take(request.PagingOptions.PageSize);
        }
        
        var result = queryable.Select(i => new RecipeGetListResponse(i.Id, i.Name, i.ImageUrl)).ToList();
        
        return Task.FromResult(Result.Ok(result));
    }
}
