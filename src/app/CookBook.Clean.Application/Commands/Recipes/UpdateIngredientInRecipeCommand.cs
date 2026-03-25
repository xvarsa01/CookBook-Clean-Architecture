using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Commands.Recipes;

public record UpdateIngredientInRecipeCommand(Guid RecipeId, Guid EntryId, decimal NewAmount, MeasurementUnit NewUnit) : ICommand;

internal sealed class UpdateIngredientInRecipeCommandHandler(IRepository<Recipe> recipeRepository) : ICommandHandler<UpdateIngredientInRecipeCommand>
{

    public async Task<Result> Handle(UpdateIngredientInRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound("Recipe not found");
        }
        
        var amountResult = IngredientAmount.CreateObject(request.NewAmount);
        if (!amountResult.IsSuccess)
        {
            return Result.Invalid<Guid>(amountResult.Error);
        }

        var result = recipe.UpdateIngredientEntry(request.EntryId, amountResult.Value, request.NewUnit);
        if (result.IsFailure)
            return Result.Invalid(result.Error);
        
        await recipeRepository.UpdateAsync(recipe);
        
        return Result.Ok();
    }
}
