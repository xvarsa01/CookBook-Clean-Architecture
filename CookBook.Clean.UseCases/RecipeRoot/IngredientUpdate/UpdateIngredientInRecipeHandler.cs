using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.IngredientUpdate;

public class UpdateIngredientInRecipeHandler(IRepository<RecipeEntity> recipeRepository) : IRequestHandler<UpdateIngredientInRecipeUseCase, UseCaseResult>
{

    public async Task<UseCaseResult> Handle(UpdateIngredientInRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return UseCaseResult.NotFound("Recipe not found");
        }

        if (request.NewAmount <= 0)
        {
            return UseCaseResult.Invalid("Amount must be positive");
        }
        
        recipe.UpdateIngredientEntry(request.EntryId, request.NewAmount, request.NewUnit);
        
        await recipeRepository.UpdateAsync(recipe);
        
        return UseCaseResult.Ok();
    }
}
