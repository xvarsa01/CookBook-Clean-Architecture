using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientNameQuery(string IngredientNameSubstring) : IQuery<List<RecipeGetListResponse>>;

internal class GetRecipeListByContainingIngredientNameQueryHandler (IRepository<Recipe, RecipeId> repository,  IMediator mediator)
    : IQueryHandler<GetRecipeListByContainingIngredientNameQuery, List<RecipeGetListResponse>>
{
    public async Task<Result<List<RecipeGetListResponse>>> Handle(GetRecipeListByContainingIngredientNameQuery request, CancellationToken cancellationToken)
    {
        var ingredientFilter = new IngredientFilter { Name = request.IngredientNameSubstring };
        var ingredientResult = await mediator.Send(new GetIngredientListQuery(ingredientFilter), cancellationToken);

        if (!ingredientResult.IsSuccess)
            return Result.Invalid<List<RecipeGetListResponse>>(ingredientResult.Error);

        var ingredientIds = ingredientResult.Value
            .Select(i => i.Id)
            .ToList();

        var result = repository.Query()
            .Where(r => r.Ingredients.Any(ri => ingredientIds.Contains(ri.IngredientId)))
            .Select(i => new RecipeGetListResponse(i.Id, i.Name, i.ImageUrl))
            .ToList();
        
        return Result.Ok(result);

    }
}
