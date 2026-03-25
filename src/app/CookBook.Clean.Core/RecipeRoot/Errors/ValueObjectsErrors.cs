namespace CookBook.Clean.Core.RecipeRoot.Errors;

public static class ValueObjectsErrors
{
    public static Error IngredientAmountNotPositiveError() => new("Amount must be positive");

    public static Error RecipeDurationNotPositiveError() => new("Duration must be positive");

    public static Error RecipeNameNotInvalidError() => new("Recipe name must be at least 3 characters.");
}
