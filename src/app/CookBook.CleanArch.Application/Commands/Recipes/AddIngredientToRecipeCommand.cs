using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.IngredientRoot;
using CookBook.CleanArch.Domain.IngredientRoot.Errors;
using CookBook.CleanArch.Domain.IngredientRoot.ValueObjects;
using CookBook.CleanArch.Domain.RecipeRoot;
using CookBook.CleanArch.Domain.RecipeRoot.Errors;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

namespace CookBook.CleanArch.Application.Commands.Recipes;

public record AddIngredientToRecipeCommand(Guid RecipeId, RecipeAddIngredientRequest Request) : ICommand<Guid>;

internal sealed class AddIngredientToRecipeCommandHandler(
    IRepository<Recipe, RecipeId> recipeRepository,
    IRepository<Ingredient, IngredientId> ingredientRepository
) : ICommandHandler<AddIngredientToRecipeCommand,Guid>
{
    public async Task<Result<Guid>> Handle(AddIngredientToRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound<Guid>(RecipeErrors.RecipeNotFoundError(new RecipeId(request.RecipeId)));
        }

        var ingredient = await ingredientRepository.GetByIdAsync(request.Request.IngredientId);
        if (ingredient is null)
        {
            return Result.NotFound<Guid>(IngredientErrors.IngredientNotFoundError(new IngredientId(request.Request.IngredientId)));
        }

        var result = recipe.AddIngredient(request.Request.IngredientId, request.Request.Amount, request.Request.Unit);
        if (!result.IsSuccess)
        {
            return Result.Invalid<Guid>(result.Error);
        }
        
        await recipeRepository.UpdateAsync(recipe);

        return Result.Ok(result.Value);
    }
}
