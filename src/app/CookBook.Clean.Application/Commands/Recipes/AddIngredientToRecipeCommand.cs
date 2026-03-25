using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Errors;
using CookBook.Clean.Core.IngredientRoot.ValueObjects;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.Errors;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Commands.Recipes;

public record AddIngredientToRecipeCommand(Guid RecipeId, RecipeAddIngredientRequest Data) : ICommand<Guid>;

internal sealed class AddIngredientToRecipeCommandHandler(
    IRepository<Recipe, RecipeId> recipeRepository,
    IRepository<Ingredient, IngredientId> ingredientRepository
) : ICommandHandler<AddIngredientToRecipeCommand,Guid>
{
    public async Task<Result<Guid>> Handle(AddIngredientToRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound<Guid>(RecipeErrors.RecipeNotFoundError(new RecipeId(request.RecipeId)));
        }

        var ingredient = await ingredientRepository.GetByIdAsync(request.Data.IngredientId);
        if (ingredient is null)
        {
            return Result.NotFound<Guid>(IngredientErrors.IngredientNotFoundError(new IngredientId(request.Data.IngredientId)));
        }

        var result = recipe.AddIngredient(request.Data.IngredientId, request.Data.Amount, request.Data.Unit);
        if (!result.IsSuccess)
        {
            return Result.Invalid<Guid>(result.Error);
        }
        
        await recipeRepository.UpdateAsync(recipe);

        return Result.Ok(result.Value);
    }
}
