using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientIdQuery(Guid IngredientId) : IQuery<List<RecipeGetListResponse>>;

internal class GetRecipeListByContainingIngredientIdQueryHandler (IRepository<Recipe, RecipeId> repository) : IQueryHandler<GetRecipeListByContainingIngredientIdQuery, List<RecipeGetListResponse>>
{
    public Task<Result<List<RecipeGetListResponse>>> Handle(GetRecipeListByContainingIngredientIdQuery request, CancellationToken cancellationToken)
    {
        var queryable = repository.Query();
        
        queryable = queryable.Where(r => r.Ingredients.Any(i => i.IngredientId == request.IngredientId));
        
        var result = queryable.Select(i => new RecipeGetListResponse(i.Id, i.Name, i.ImageUrl)).ToList();
        
        return Task.FromResult(Result.Ok(result));
    }
}
