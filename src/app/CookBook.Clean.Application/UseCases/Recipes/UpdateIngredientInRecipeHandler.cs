using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.Exceptions;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record UpdateIngredientInRecipeUseCase(Guid RecipeId, Guid EntryId, decimal NewAmount, MeasurementUnit NewUnit) : IRequest<Result>;

internal class UpdateIngredientInRecipeHandler(IRepository<RecipeEntity> recipeRepository) : IRequestHandler<UpdateIngredientInRecipeUseCase, Result>
{

    public async Task<Result> Handle(UpdateIngredientInRecipeUseCase request, CancellationToken cancellationToken)
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

        try
        {
            recipe.UpdateIngredientEntry(request.EntryId, amountResult.Value, request.NewUnit);
        }
        catch (RecipeIngredientByEntryIdNotFoundException ex)
        {
            return Result.Invalid(ex.Message);
        }
        
        await recipeRepository.UpdateAsync(recipe);
        
        return Result.Ok();
    }
}
