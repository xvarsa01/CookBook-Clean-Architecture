using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.IngredientUpdate;

public record UpdateIngredientInRecipeUseCaseResult;

public class UpdateIngredientInRecipeHandler(IRepository<RecipeEntity> recipeRepository) : IRequestHandler<UpdateIngredientInRecipeUseCase, UseCaseResult<UpdateIngredientInRecipeUseCaseResult>>
{

    public async Task<UseCaseResult<UpdateIngredientInRecipeUseCaseResult>> Handle(UpdateIngredientInRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return UseCaseResult<UpdateIngredientInRecipeUseCaseResult>.NotFound("Recipe not found");
        }

        if (request.NewAmount <= 0)
        {
            return UseCaseResult<UpdateIngredientInRecipeUseCaseResult>.Invalid("Amount must be positive");
        }
        
        recipe.UpdateIngredientEntry(request.EntryId, request.NewAmount, request.NewUnit);
        
        await recipeRepository.UpdateAsync(recipe);
        
        return UseCaseResult<UpdateIngredientInRecipeUseCaseResult>.Ok(new UpdateIngredientInRecipeUseCaseResult());
    }
}
