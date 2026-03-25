using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.IngredientRoot;
using CookBook.CleanArch.Domain.IngredientRoot.ValueObjects;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

namespace CookBook.CleanArch.Application.Queries.Ingredients;

public record GetIngredientListQuery(IngredientFilter Filter, PagingOptions? PagingOptions = null) : IQuery<List<IngredientGetListResponse>>;

internal class GetIngredientListQueryHandler (IRepository<Ingredient, IngredientId> repository) : IQueryHandler<GetIngredientListQuery, List<IngredientGetListResponse>>
{
    public Task<Result<List<IngredientGetListResponse>>> Handle(GetIngredientListQuery request, CancellationToken cancellationToken)
    {
        var queryable = repository.Query();
        
        if (request.Filter.Name is not null)
        {
            queryable = queryable.Where(i => i.Name.ToLower().Contains(request.Filter.Name.ToLower()));
        }

        if (request.Filter.HasDescription is not null)
        {
            queryable = request.Filter.HasDescription.Value
                ? queryable.Where(i => i.Description != null)
                : queryable.Where(i => i.Description == null);
        }

        if (request.Filter.HasImage is not null)
        {
            queryable = request.Filter.HasImage.Value
                ? queryable.Where(i => i.ImageUrl != null)
                : queryable.Where(i => i.ImageUrl == null);
        }
        
        queryable = request.Filter.SortParameter switch
        {
            IngredientsSortParameter.Name => request.Filter.IsSortAscending
                ? queryable.OrderBy(r => r.Name)
                : queryable.OrderByDescending(r => r.Name),
            
            _ => queryable.OrderBy(r => r.Name)
        };
        
        if (request.PagingOptions is not null)
        {
            queryable = queryable
                .Skip(request.PagingOptions.PageSize * request.PagingOptions.PageIndex)
                .Take(request.PagingOptions.PageSize);
        }
        var result = queryable.Select(i => new IngredientGetListResponse(i.Id, i.Name, i.ImageUrl)).ToList();
        
        return Task.FromResult(Result.Ok(result));
    }
}
