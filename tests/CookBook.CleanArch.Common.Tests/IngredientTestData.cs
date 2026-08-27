using CookBook.CleanArch.Domain.Ingredients;

namespace CookBook.CleanArch.Common.Tests;

public static class IngredientTestData
{
    public static Ingredient CreateIngredient() =>
        Ingredient.Create(
            name: "ingredient",
            description: null,
            imageUrl: null).Value;
}
