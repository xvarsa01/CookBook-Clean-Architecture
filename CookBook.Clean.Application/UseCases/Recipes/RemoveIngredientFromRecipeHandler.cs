using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

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
