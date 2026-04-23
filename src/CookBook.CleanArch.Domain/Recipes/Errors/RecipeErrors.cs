using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Recipes.Errors;

public static class RecipeErrors
{
    public static Error RecipeNotFoundError(RecipeId recipeId) => new($"Recipe {recipeId} not found");
    
    public static Error RecipeIngredientByEntryIdNotFoundError(RecipeIngredientId recipeIngredientEntryId, RecipeId recipeId) =>
        new($"Ingredient entry for {recipeIngredientEntryId.Value} not found in recipe {recipeId.Value}.");

    public static Error RecipeIngredientByIdNotFoundError(IngredientId ingredientId, RecipeId recipeId) =>
        new($"Ingredient {ingredientId} not found in recipe {recipeId}.");
    
    public static Error RecipeMaximumNumberOfIngredientsError(RecipeId recipeId) =>
        new($"Recipe {recipeId} can not have more than 10 ingredients.");

    public static Error RecipeMinimumNumberOfIngredientsError(RecipeId recipeId) =>
        new($"Recipe {recipeId} must contain at least 1 ingredient.");
    
    public static Error RecipeNoIngredientsError() => new($"Recipe must contain at least 1 ingredient.");
}

