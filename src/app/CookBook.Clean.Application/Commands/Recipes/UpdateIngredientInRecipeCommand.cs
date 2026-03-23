using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record UpdateIngredientInRecipeCommand(Guid RecipeId, Guid EntryId, decimal NewAmount, MeasurementUnit NewUnit) : ICommand;

internal sealed class UpdateIngredientInRecipeCommandHandler(IRepository<RecipeEntity> recipeRepository) : ICommandHandler<UpdateIngredientInRecipeCommand>
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
            return Result.Invalid<Guid>(amountResult.Error ?? string.Empty);
        }

        var result = recipe.UpdateIngredientEntry(request.EntryId, amountResult.Value, request.NewUnit);
        if (result.IsFailure)
            return Result.Invalid(result.Error ?? string.Empty);
        
        await recipeRepository.UpdateAsync(recipe);
        
        return Result.Ok();
    }
}
