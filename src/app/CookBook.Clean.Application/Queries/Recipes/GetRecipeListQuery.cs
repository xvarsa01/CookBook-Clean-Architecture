using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListQuery(RecipeFilter Filter, PagingOptions? PagingOptions = null) : IQuery<List<RecipeGetListDto>>;

internal class GetRecipeListQueryHandler(IRepository<RecipeEntity> repository) : IQueryHandler<GetRecipeListQuery, List<RecipeGetListDto>>
{
    public Task<Result<List<RecipeGetListDto>>> Handle(GetRecipeListQuery request, CancellationToken cancellationToken)
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
            var minTimeStr = request.Filter.MinimalDuration.Value.ToString(@"hh\:mm\:ss");
            queryable = queryable.Where(r => string.Compare(r.Duration.ToString(), minTimeStr) >= 0);       // workaround for sqlite
        }

        if (request.Filter.MaximalDuration.HasValue)
        {
            var maxTimeStr = request.Filter.MaximalDuration.Value.ToString(@"hh\:mm\:ss");
            queryable = queryable.Where(r => string.Compare(r.Duration.ToString(), maxTimeStr) <= 0);
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
                ? queryable.OrderBy(r => r.Duration.ToString())                 // .ToString() is workaround for sqlite
                : queryable.OrderByDescending(r => r.Duration.ToString()),
            
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
        
        var result = queryable.Select(i => new RecipeGetListDto()
        {
            Id = i.Id,
            Name = i.Name,
            ImageUrl = i.ImageUrl
        }).ToList();
        
        return Task.FromResult(Result.Ok(result));
    }
}
