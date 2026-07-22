using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record UpdateRecipeWithIngredientsCommand(RecipeUpdateWithIngredientsRequest Request) : ICommand<RecipeId>;

internal sealed class UpdateRecipeWithIngredientsCommandHandler(
    IRepository<Recipe, RecipeId> recipeRepository,
    IRepository<Ingredient, IngredientId> ingredientRepository)
    : ICommandHandler<UpdateRecipeWithIngredientsCommand, RecipeId>
{
    public async Task<Result<RecipeId>> Handle(UpdateRecipeWithIngredientsCommand request, CancellationToken cancellationToken)
    {
        var recipeRequest = request.Request;
        var recipe = await recipeRepository.GetByIdAsync(recipeRequest.Id);
        if (recipe is null)
            return Result.Failure<RecipeId>(RecipeErrors.RecipeNotFoundError(recipeRequest.Id));

        if (recipeRequest.Name is not null)
        {
            var result = recipe.UpdateName(recipeRequest.Name);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }
        
        if (recipeRequest.Description is not null)
        {
            var result = recipe.UpdateDescription(recipeRequest.Description);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }

        if (recipeRequest.ImageUrl is not null ||
            recipeRequest.Duration is not null ||
            recipeRequest.Type is not null)
        {
            var result = recipe.UpdateRest(
                recipeRequest.ImageUrl ?? recipe.ImageUrl,
                recipeRequest.Duration,
                recipeRequest.Type);

            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }

        if (recipeRequest.Ingredients is not null)
        {
            List<RecipeCreateIngredient> ingredients = [];
            foreach (var ingredientRequest in recipeRequest.Ingredients)
            {
                var ingredient = await ingredientRepository.GetByIdAsync(ingredientRequest.IngredientId);
                if (ingredient is null)
                    return Result.Failure<RecipeId>(IngredientErrors.IngredientNotFoundError(ingredientRequest.IngredientId));

                ingredients.Add(new RecipeCreateIngredient(ingredientRequest.IngredientId, ingredientRequest.Amount, ingredientRequest.Unit));
            }

            var result = recipe.UpdateIngredients(ingredients);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }

        return Result.Success(recipe.Id);
    }
}
