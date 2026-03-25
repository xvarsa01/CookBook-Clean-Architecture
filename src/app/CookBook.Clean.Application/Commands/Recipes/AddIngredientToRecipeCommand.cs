using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Commands.Recipes;

public record AddIngredientToRecipeCommand(Guid RecipeId, Guid IngredientId, decimal Amount, MeasurementUnit Unit) : ICommand<Guid>;

internal sealed class AddIngredientToRecipeCommandHandler(
    IRepository<Recipe> recipeRepository,
    IRepository<Ingredient> ingredientRepository
) : ICommandHandler<AddIngredientToRecipeCommand,Guid>
{
    public async Task<Result<Guid>> Handle(AddIngredientToRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound<Guid>("Recipe not found");
        }

        var ingredient = await ingredientRepository.GetByIdAsync(request.IngredientId);
        if (ingredient is null)
        {
            return Result.NotFound<Guid>("Ingredient not found");
        }

        var amountResult = IngredientAmount.CreateObject(request.Amount);
        if (!amountResult.IsSuccess)
        {
            return Result.Invalid<Guid>(amountResult.Error);
        }
        
        var result = recipe.AddIngredient(request.IngredientId, amountResult.Value, request.Unit);
        if (!result.IsSuccess)
        {
            return Result.Invalid<Guid>(result.Error);
        }
        
        await recipeRepository.UpdateAsync(recipe);

        return Result.Ok(result.Value);
    }
}
