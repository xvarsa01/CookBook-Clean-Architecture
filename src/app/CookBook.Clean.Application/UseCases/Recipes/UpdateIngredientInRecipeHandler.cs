using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Exceptions;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

internal class UpdateIngredientInRecipeHandler(IRepository<RecipeEntity> recipeRepository) : IRequestHandler<UpdateIngredientInRecipeUseCase, UseCaseResult>
{

    public async Task<UseCaseResult> Handle(UpdateIngredientInRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return UseCaseResult.NotFound("Recipe not found");
        }

        IngredientAmount amount;
        try
        {
            amount = new IngredientAmount(request.NewAmount);
        }
        catch (ArgumentException ex)
        {
            return UseCaseResult.Invalid(ex.Message);
        }

        try
        {
            recipe.UpdateIngredientEntry(request.EntryId, amount, request.NewUnit);
        }
        catch (RecipeIngredientByEntryIdNotFoundException ex)
        {
            return UseCaseResult.Invalid(ex.Message);
        }
        
        await recipeRepository.UpdateAsync(recipe);
        
        return UseCaseResult.Ok();
    }
}
