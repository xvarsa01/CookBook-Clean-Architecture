using CookBook.CleanArch.Domain.Ingredients.ValueObjects;

namespace CookBook.CleanArch.Domain.Ingredients.Errors;

public static class IngredientErrors
{
    public static Error IngredientNotFoundError(IngredientId ingredientId) => new($"Ingredient {ingredientId} not found");
    public static Error IngredientNameEmptyError() => new("Ingredient name can not be empty.");
    public static Error IngredientIsUsedAndCanNotBeDeletedError(int valueCount) => new($"Cannot delete ingredient that is used in {valueCount} recipes. Remove it from all recipes first.");
}

