using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

internal class AddIngredientToRecipeHandler(
    IRepository<RecipeEntity> recipeRepository,
    IRepository<IngredientEntity> ingredientRepository
) : IRequestHandler<AddIngredientToRecipeUseCase, UseCaseResult<Guid>>
{
    public async Task<UseCaseResult<Guid>> Handle(AddIngredientToRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return UseCaseResult<Guid>.NotFound("Recipe not found");
        }

        var ingredient = await ingredientRepository.GetByIdAsync(request.IngredientId);
        if (ingredient is null)
        {
            return UseCaseResult<Guid>.NotFound("Ingredient not found");
        }

        var id = recipe.AddIngredient(request.IngredientId, request.Amount, request.Unit);
        await recipeRepository.UpdateAsync(recipe);

        return UseCaseResult<Guid>.Ok(id);
    }
}
