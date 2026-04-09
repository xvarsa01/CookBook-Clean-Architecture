using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Ingredient.Errors;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.Errors;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Commands;

public record AddIngredientToRecipeCommand(RecipeId RecipeId, RecipeAddIngredientRequest Request) : ICommand<RecipeIngredientId>;

internal sealed class AddIngredientToRecipeCommandHandler(
    IRepository<Recipe, RecipeId> recipeRepository,
    IRepository<Ingredient, IngredientId> ingredientRepository
) : ICommandHandler<AddIngredientToRecipeCommand,RecipeIngredientId>
{
    public async Task<Result<RecipeIngredientId>> Handle(AddIngredientToRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound<RecipeIngredientId>(RecipeErrors.RecipeNotFoundError(request.RecipeId));
        }

        var ingredient = await ingredientRepository.GetByIdAsync(request.Request.IngredientId);
        if (ingredient is null)
        {
            return Result.NotFound<RecipeIngredientId>(IngredientErrors.IngredientNotFoundError(new IngredientId(request.Request.IngredientId)));
        }

        var result = recipe.AddIngredient(request.Request.IngredientId, request.Request.Amount, request.Request.Unit);
        if (!result.IsSuccess)
        {
            return Result.Invalid<RecipeIngredientId>(result.Error);
        }
        
        return Result.Ok(result.Value);
    }
}
