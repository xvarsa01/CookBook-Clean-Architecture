using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientListQuery(IngredientFilter Filter, PagingOptions? PagingOptions = null) : IQuery<List<IngredientGetListDto>>;

internal class GetIngredientListQueryHandler (IRepository<Ingredient> repository) : IQueryHandler<GetIngredientListQuery, List<IngredientGetListDto>>
{
    public Task<Result<List<IngredientGetListDto>>> Handle(GetIngredientListQuery request, CancellationToken cancellationToken)
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
        var result = queryable.Select(i => new IngredientGetListDto()
        {
            Id = i.Id,
            Name = i.Name,
            ImageUrl = i.ImageUrl
        }).ToList();
        
        return Task.FromResult(Result.Ok(result));
    }
}
