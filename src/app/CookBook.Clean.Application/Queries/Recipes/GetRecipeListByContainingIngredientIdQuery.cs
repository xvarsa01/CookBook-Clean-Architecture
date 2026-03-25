using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientIdQuery(Guid IngredientId) : IQuery<List<RecipeGetListDto>>;

internal class GetRecipeListByContainingIngredientIdQueryHandler (IRepository<Recipe> repository) : IQueryHandler<GetRecipeListByContainingIngredientIdQuery, List<RecipeGetListDto>>
{
    public Task<Result<List<RecipeGetListDto>>> Handle(GetRecipeListByContainingIngredientIdQuery request, CancellationToken cancellationToken)
    {
        var queryable = repository.Query();
        
        queryable = queryable.Where(r => r.Ingredients.Any(i => i.IngredientId == request.IngredientId));
        
        var result = queryable.Select(i => new RecipeGetListDto()
        {
            Id = i.Id,
            Name = i.Name,
            ImageUrl = i.ImageUrl
        }).ToList();
        
        return Task.FromResult(Result.Ok(result));
    }
}
