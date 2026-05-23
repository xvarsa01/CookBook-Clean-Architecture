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

public record UpdateRecipeWithIngredientsCommand(
    RecipeUpdateWithIngredientsRequest Request
) : ICommand<RecipeId>;

internal sealed class UpdateRecipeWithIngredientsCommandHandler(
    IRepository<Recipe, RecipeId> recipeRepository,
    IRepository<Ingredient, IngredientId> ingredientRepository ) : ICommandHandler<UpdateRecipeWithIngredientsCommand, RecipeId>
{
    private const int MinIngredients = Recipe.MinIngredients;
    private const int MaxIngredients = Recipe.MaxIngredients;
    
    public async Task<Result<RecipeId>> Handle(UpdateRecipeWithIngredientsCommand request, CancellationToken cancellationToken)
    {
        var recipeRequest = request.Request;
        var ingredientUpdatesList = (recipeRequest.Updates ?? []).ToList();
        var ingredientAdditionsList = (recipeRequest.Additions ?? []).ToList();
        var ingredientRemovalsList = (recipeRequest.Removals ?? []).ToList();
        
        var recipe = await recipeRepository.GetByIdAsync(recipeRequest.Id);
        if (recipe is null)
        {
            return Result.Failure<RecipeId>(RecipeErrors.RecipeNotFoundError(recipeRequest.Id));
        }

        // Pre-validation checks
        if (ingredientUpdatesList.Any(u => ingredientRemovalsList.Contains(u.EntryId)))
        {
            return Result.Failure<RecipeId>(
                RecipeErrors.RecipeIngredientByEntryIdNotFoundError(
                    ingredientUpdatesList.First(u => ingredientRemovalsList.Contains(u.EntryId)).EntryId,
                    recipe.Id));
        }

        var finalCount = recipe.Ingredients.Count - ingredientRemovalsList.Count + ingredientAdditionsList.Count;
        if (finalCount < MinIngredients || finalCount > MaxIngredients)
        {
            return Result.Failure<RecipeId>(
                finalCount > MaxIngredients
                    ? RecipeErrors.RecipeMaximumNumberOfIngredientsError(recipe.Id)
                    : RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id));
        }

        // Recipe properties update
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
        
        var restResult = recipe.UpdateRest(recipeRequest.ImageUrl, recipeRequest.Duration, recipeRequest.Type);
        if (restResult.IsFailure)
            return Result.Failure<RecipeId>(restResult.Error);

        // Update ingredient properties
        foreach (var update in ingredientUpdatesList)
        {
            var result = recipe.UpdateIngredientEntry(update.EntryId, update.NewAmount, update.NewUnit);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }
        
        var currentCount = recipe.Ingredients.Count;

        while (ingredientAdditionsList.Count > 0 || ingredientRemovalsList.Count > 0)
        {
            // If at minimum, add first; otherwise prefer removals.
            var shouldAdd = (currentCount <= MinIngredients && ingredientAdditionsList.Count > 0)
                || (ingredientRemovalsList.Count == 0 && ingredientAdditionsList.Count > 0)
                || (currentCount >= MaxIngredients && ingredientRemovalsList.Count == 0 && ingredientAdditionsList.Count > 0);

            if (shouldAdd)
            {
                var addition = ingredientAdditionsList[0];
                ingredientAdditionsList.RemoveAt(0);

                var ingredient = await ingredientRepository.GetByIdAsync(addition.IngredientId);
                if (ingredient is null)
                {
                    return Result.Failure<RecipeId>(
                        IngredientErrors.IngredientNotFoundError(addition.IngredientId));
                }

                var addResult = recipe.AddIngredient(addition.IngredientId, addition.Amount, addition.Unit);
                if (addResult.IsFailure)
                    return Result.Failure<RecipeId>(addResult.Error);

                currentCount++;
                continue;
            }

            if (ingredientRemovalsList.Count > 0)
            {
                var entryId = ingredientRemovalsList[0];
                ingredientRemovalsList.RemoveAt(0);

                var removeResult = recipe.RemoveIngredientByEntryId(entryId);
                if (removeResult.IsFailure)
                    return Result.Failure<RecipeId>(removeResult.Error);

                currentCount--;
            }
        }

        return Result.Success(recipe.Id);
    }
}

