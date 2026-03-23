using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Exceptions;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

internal class AddIngredientToRecipeHandler(
    IRepository<RecipeEntity> recipeRepository,
    IRepository<IngredientEntity> ingredientRepository
) : IRequestHandler<AddIngredientToRecipeUseCase, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddIngredientToRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound<Guid>("Recipe not found");
        }

        var ingredient = await ingredientRepository.GetByIdAsync(request.IngredientId);
        if (ingredient is null)
        {
            return Result.NotFound<Guid>("Ingredient not found");
        }

        Guid id;
        try
        {
            id = recipe.AddIngredient(request.IngredientId, new IngredientAmount(request.Amount), request.Unit);
        }
        catch (RecipeMaximumNumberOfIngredients ex)
        {
            return Result.Invalid<Guid>(ex.Message);
        }
        
        await recipeRepository.UpdateAsync(recipe);

        return Result.Ok(id);
    }
}
