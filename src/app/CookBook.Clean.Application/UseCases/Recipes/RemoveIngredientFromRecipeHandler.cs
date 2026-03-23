using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Exceptions;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

internal class RemoveIngredientFromRecipeHandler(IRepository<RecipeEntity> recipeRepository)
    : IRequestHandler<RemoveIngredientFromRecipeUseCase, Result>
{
    public async Task<Result> Handle(RemoveIngredientFromRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound("Recipe not found");
        }
        
        try
        {
            recipe.RemoveIngredientByEntryId(request.EntryId);
        }
        catch (RecipeIngredientByEntryIdNotFoundException ex)
        {
            return Result.Invalid(ex.Message);
        }

        await recipeRepository.UpdateAsync(recipe);
        return Result.Ok();
    }
}
