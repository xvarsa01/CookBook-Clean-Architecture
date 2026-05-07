namespace CookBook.CleanArch.Domain.Recipes.Errors;

public static class RecipeValueObjectsErrors
{
    public static Error IngredientAmountNotPositiveError() =>
        new("RecipeValueObjectsErrors.IngredientAmountNotPositive", "Amount must be positive");

    public static Error RecipeDurationNotPositiveError() =>
        new("RecipeValueObjectsErrors.RecipeDurationNotPositive", "Duration must be positive");

    public static Error RecipeNameNotInvalidError() =>
        new("RecipeValueObjectsErrors.RecipeNameTooShort", "Recipe name must be at least 3 characters.");
}
