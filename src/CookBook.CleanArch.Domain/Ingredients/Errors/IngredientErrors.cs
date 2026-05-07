using CookBook.CleanArch.Domain.Ingredients.ValueObjects;

namespace CookBook.CleanArch.Domain.Ingredients.Errors;

public static class IngredientErrors
{
    public static Error IngredientNotFoundError(IngredientId ingredientId) =>
        new("Ingredients.IngredientNotFound", $"Ingredient {ingredientId} not found", ingredientId);
    public static Error IngredientNameEmptyError() => new("Ingredients.NameEmpty", "Ingredient name can not be empty.");
    public static Error IngredientIsUsedAndCanNotBeDeletedError(int valueCount) =>
        new("Ingredients.InUseCannotDelete", $"Cannot delete ingredient that is used in {valueCount} recipes. Remove it from all recipes first.", valueCount);
}

