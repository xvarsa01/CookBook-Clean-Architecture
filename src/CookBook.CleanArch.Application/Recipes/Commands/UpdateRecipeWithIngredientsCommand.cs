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
    RecipeUpdateRequest Request,
    IReadOnlyCollection<RecipeAddIngredientRequest> Additions,
    IReadOnlyCollection<RecipeUpdateIngredientRequest> Updates,
    IReadOnlyCollection<RecipeIngredientId> Removals
) : ICommand<RecipeId>;

internal sealed class UpdateRecipeWithIngredientsCommandHandler(
    IRepository<Recipe, RecipeId> recipeRepository,
    IRepository<Ingredient, IngredientId> ingredientRepository ) : ICommandHandler<UpdateRecipeWithIngredientsCommand, RecipeId>
{
    public async Task<Result<RecipeId>> Handle(UpdateRecipeWithIngredientsCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.Request.Id);
        if (recipe is null)
        {
            return Result.Failure<RecipeId>(RecipeErrors.RecipeNotFoundError(request.Request.Id));
        }

        // Pre-validation: check for conflicts between updates and removals
        if (request.Updates.Any(u => request.Removals.Contains(u.EntryId)))
        {
            return Result.Failure<RecipeId>(
                RecipeErrors.RecipeIngredientByEntryIdNotFoundError(
                    request.Updates.First(u => request.Removals.Contains(u.EntryId)).EntryId,
                    recipe.Id));
        }

        var finalCount = recipe.Ingredients.Count - request.Removals.Count + request.Additions.Count;
        if (finalCount < Recipe.MinIngredients || finalCount > Recipe.MaxIngredients)
        {
            return Result.Failure<RecipeId>(
                finalCount > Recipe.MaxIngredients
                    ? RecipeErrors.RecipeMaximumNumberOfIngredientsError(recipe.Id)
                    : RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id));
        }

        if (request.Request.Name is not null)
        {
            var result = recipe.UpdateName(request.Request.Name);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }
        
        if (request.Request.Description is not null)
        {
            var result = recipe.UpdateDescription(request.Request.Description);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }
        
        var restResult = recipe.UpdateRest(request.Request.ImageUrl, request.Request.Duration, request.Request.Type);
        if (restResult.IsFailure)
            return Result.Failure<RecipeId>(restResult.Error);


        // Step 1: Remove ingredients
        foreach (var entryId in request.Removals)
        {
            var result = recipe.RemoveIngredientByEntryId(entryId);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }

        // Step 2: Update ingredients
        foreach (var update in request.Updates)
        {
            var result = recipe.UpdateIngredientEntry(update.EntryId, update.NewAmount, update.NewUnit);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }

        // Step 3: Add ingredients
        foreach (var addition in request.Additions)
        {
            var ingredient = await ingredientRepository.GetByIdAsync(addition.IngredientId);
            if (ingredient is null)
            {
                return Result.Failure<RecipeId>(
                    IngredientErrors.IngredientNotFoundError(addition.IngredientId));
            }

            var result = recipe.AddIngredient(addition.IngredientId, addition.Amount, addition.Unit);
            if (result.IsFailure)
                return Result.Failure<RecipeId>(result.Error);
        }

        return Result.Success(recipe.Id);
    }
}

