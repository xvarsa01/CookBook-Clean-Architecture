using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Recipes.Errors;

public static class RecipeErrors
{
    public static Error RecipeNotFoundError(RecipeId recipeId) =>
        new("Recipes.RecipeNotFound", $"Recipe {recipeId} not found", recipeId);

    public static Error RecipeIngredientByEntryIdNotFoundError(RecipeIngredientId recipeIngredientEntryId, RecipeId recipeId) =>
        new("Recipes.RecipeIngredientEntryNotFound", $"Ingredient entry for {recipeIngredientEntryId.Value} not found in recipe {recipeId.Value}.");

    public static Error RecipeIngredientByIdNotFoundError(IngredientId ingredientId, RecipeId recipeId) =>
        new("Recipes.RecipeIngredientNotFound", $"Ingredient {ingredientId} not found in recipe {recipeId}.", [ingredientId, recipeId]);

    public static Error RecipeReviewNotFoundError(RecipeReviewId reviewId, RecipeId recipeId) =>
        new("Recipes.RecipeReviewNotFound", $"Review {reviewId.Value} not found in recipe {recipeId.Value}.", [reviewId, recipeId]);

    public static Error RecipeReviewMarkOutOfRangeError(int mark) =>
        new("Recipes.RecipeReviewMarkOutOfRange", $"Review mark must be between {Recipe.MinReviewMark} and {Recipe.MaxReviewMark}.", mark);

    public static Error RecipeReviewDescriptionRequiredError() =>
        new("Recipes.RecipeReviewDescriptionRequired", "Review description is required.");

    public static Error RecipeReviewDescriptionTooLongError() =>
        new("Recipes.RecipeReviewDescriptionTooLong", $"Review description can not be longer than {Recipe.MaxReviewDescriptionLength} characters.");
    
    public static Error RecipeMaximumNumberOfIngredientsError(RecipeId? recipeId = null)
    {
        return recipeId == null
            ? new Error("Recipes.MaximumNumberOfIngredientsExceeded", $"Recipe can not have more than 10 ingredients.")
            : new Error("Recipes.MaximumNumberOfIngredientsExceeded.ForRecipe", $"Recipe {recipeId} can not have more than 10 ingredients.", recipeId);
    }

    public static Error RecipeMinimumNumberOfIngredientsError(RecipeId? recipeId = null)
    {
        return recipeId == null
            ? new Error("Recipes.MinimumNumberOfIngredientsRequired", $"Recipe must contain at least 1 ingredient.")
            : new Error("Recipes.MinimumNumberOfIngredientsRequired.ForRecipe", $"Recipe {recipeId} must contain at least 1 ingredient.", recipeId);
    }
    
    public static Error RecipeTypeNotSelectedError() =>
        new("Recipes.RecipeTypeMustBeSelected", "The recipe type must be selected");
}
