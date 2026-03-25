using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.IngredientRoot;
using CookBook.CleanArch.Domain.RecipeRoot;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;
using MediatR;

namespace CookBook.CleanArch.Application.Queries.Recipes;

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
