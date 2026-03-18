using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.RemoveIngredient;

public class RemoveIngredientFromRecipeHandler(IRepository<RecipeEntity> recipeRepository)
    : IRequestHandler<RemoveIngredientFromRecipeUseCase, UseCaseResult>
{
    public async Task<UseCaseResult> Handle(RemoveIngredientFromRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return UseCaseResult.NotFound("Recipe not found");
        }
        
        recipe.RemoveIngredientEntry(request.EntryId);

        await recipeRepository.UpdateAsync(recipe);
        return UseCaseResult.Ok();
    }
}
