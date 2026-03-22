using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Exceptions;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

internal class RemoveIngredientFromRecipeHandler(IRepository<RecipeEntity> recipeRepository)
    : IRequestHandler<RemoveIngredientFromRecipeUseCase, UseCaseResult>
{
    public async Task<UseCaseResult> Handle(RemoveIngredientFromRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return UseCaseResult.NotFound("Recipe not found");
        }
        
        try
        {
            recipe.RemoveIngredientByEntryId(request.EntryId);
        }
        catch (RecipeIngredientByEntryIdNotFoundException ex)
        {
            return UseCaseResult.Invalid(ex.Message);
        }

        await recipeRepository.UpdateAsync(recipe);
        return UseCaseResult.Ok();
    }
}
