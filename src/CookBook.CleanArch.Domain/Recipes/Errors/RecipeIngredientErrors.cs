namespace CookBook.CleanArch.Domain.Recipes.Errors;

public static class RecipeIngredientErrors
{
    public static Error IngredientNotSelectedError() =>
        new("RecipeIngredient.IngredientMustBeSelected", "The ingredient must be selected");

    public static Error UnitNotSelectedError() =>
        new("RecipeIngredient.UnitMustBeSelected", "The unit must be selected");

}
