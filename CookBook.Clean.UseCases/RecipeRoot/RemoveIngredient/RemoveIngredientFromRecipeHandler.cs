using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.RemoveIngredient;

public record RemoveIngredientFromRecipeResult;

public class RemoveIngredientFromRecipeHandler(IRepository<RecipeEntity> recipeRepository)
    : IRequestHandler<RemoveIngredientFromRecipeUseCase, UseCaseResult<RemoveIngredientFromRecipeResult>>
{
    public async Task<UseCaseResult<RemoveIngredientFromRecipeResult>> Handle(RemoveIngredientFromRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return UseCaseResult<RemoveIngredientFromRecipeResult>.NotFound("Recipe not found");
        }
        
        recipe.RemoveIngredientEntry(request.EntryId);

        await recipeRepository.UpdateAsync(recipe);
        return UseCaseResult<RemoveIngredientFromRecipeResult>.Ok(new RemoveIngredientFromRecipeResult());
    }
}
