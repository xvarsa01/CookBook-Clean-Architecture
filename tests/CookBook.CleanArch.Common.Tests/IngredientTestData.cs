using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Common.Tests;

public static class IngredientTestData
{
    private const string ValidImageUrl = "https://example.com/ingredient.png";

    public static Ingredient CreateIngredient(
        string name = "ingredient",
        string? description = null,
        ImageUrl? imageUrl = null) =>
        Ingredient.Create(name, description, imageUrl).Value;

    public static IngredientTestDataSet CreateSet()
    {
        var imageUrl = ImageUrl.CreateObject(ValidImageUrl).Value;
        var water = CreateIngredient("water", "water simply does not need a description", imageUrl);
        var lemon = CreateIngredient("lemon", imageUrl: imageUrl);
        var unused = CreateIngredient("unused ingredient");
        List<Ingredient> paginationIngredients = [];
        for (var i = 1; i <= 10; i++)
        {
            paginationIngredients.Add(CreateIngredient($"pagination ingredient {i}"));
        }

        IReadOnlyList<Ingredient> all = [water, lemon, unused, .. paginationIngredients];

        return new IngredientTestDataSet(
            water,
            lemon,
            unused,
            all);
    }
}

public sealed record IngredientTestDataSet(
    Ingredient Water,
    Ingredient Lemon,
    Ingredient Unused,
    IReadOnlyList<Ingredient> All);
