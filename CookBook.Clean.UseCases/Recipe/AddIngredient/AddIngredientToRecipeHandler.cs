using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.AddIngredient;

public record AddIngredientToRecipeResult(Guid CreatedEntryId);

public class AddIngredientToRecipeHandler(
    IRepository<RecipeEntity> recipeRepository,
    IRepository<IngredientEntity> ingredientRepository
) : IRequestHandler<AddIngredientToRecipeUseCase, UseCaseResult<AddIngredientToRecipeResult>>
{
    public async Task<UseCaseResult<AddIngredientToRecipeResult>> Handle(AddIngredientToRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return UseCaseResult<AddIngredientToRecipeResult>.NotFound("Recipe not found");
        }

        var ingredient = await ingredientRepository.GetByIdAsync(request.IngredientId);
        if (ingredient is null)
        {
            return UseCaseResult<AddIngredientToRecipeResult>.NotFound("Ingredient not found");
        }

        var id = recipe.AddIngredient(request.IngredientId, request.Amount, request.Unit);
        await recipeRepository.UpdateAsync(recipe);

        return UseCaseResult<AddIngredientToRecipeResult>.Ok(new AddIngredientToRecipeResult(id));
    }
}
