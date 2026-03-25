using CookBook.Clean.Core.IngredientRoot.ValueObjects;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Core.RecipeRoot.Errors;

public static class RecipeErrors
{
    public static Error RecipeNotFoundError(RecipeId recipeId) => new($"Recipe {recipeId} not found");
    
    public static Error RecipeHasNoIngredientsError(RecipeId recipeId) => new($"Recipe entity {recipeId} has no ingredients.");

    public static Error RecipeIngredientByEntryIdNotFoundError(IngredientInRecipeId ingredientEntryId, RecipeId recipeId) =>
        new($"Ingredient entry for {ingredientEntryId.Id} not found in recipe {recipeId.Id}.");

    public static Error RecipeIngredientByIdNotFoundError(IngredientId ingredientId, RecipeId recipeId) =>
        new($"Ingredient {ingredientId} not found in recipe {recipeId}.");
    
    public static Error RecipeMaximumNumberOfIngredientsError(RecipeId recipeId) =>
        new($"Recipe {recipeId} can not have more than 10 ingredients.");
    
    public static Error RecipeUpdateFailedError(RecipeId recipeId) => new($"Recipe {recipeId} update failed");
}

