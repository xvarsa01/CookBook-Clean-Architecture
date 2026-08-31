using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Common.Tests;

public static class RecipeTestData
{
    public static Recipe CreateRecipe(string name = "recipe",
        string? description = null,
        ImageUrl? imageUrl = null,
        TimeSpan? duration = null,
        RecipeType type = RecipeType.Other,
        IReadOnlyCollection<RecipeIngredientData>? ingredients = null)
    {
        ingredients ??=
        [
            new RecipeIngredientData(
                IngredientTestData.CreateIngredient().Id,
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces)
        ];

        return Recipe.Create(
            RecipeName.CreateObject(name).Value,
            description,
            imageUrl,
            RecipeDuration.CreateObject(duration ?? TimeSpan.FromMinutes(10)).Value,
            type,
            ingredients).Value;
    }

    public static RecipeTestDataSet CreateSet(IngredientTestDataSet ingredients)
    {
        var imageUrl = ImageUrl.CreateObject("https://example.com/recipe.png").Value;
        var onePiece = IngredientAmount.CreateObject(1).Value;
        var oneHundredMl = IngredientAmount.CreateObject(100).Value;
        var fiveHundredMl = IngredientAmount.CreateObject(500).Value;

        var withSingleIngredient = CreateRecipe(
            name: "minimalistic",
            description: "simple recipe",
            imageUrl: imageUrl,
            duration: TimeSpan.FromMinutes(3),
            type: RecipeType.Caffe, 
            ingredients: [new RecipeIngredientData(ingredients.Water.Id, oneHundredMl, MeasurementUnit.Ml)]);
        withSingleIngredient.AddReview(5, "Excellent recipe.");
        withSingleIngredient.AddReview(3, "Fine.");

        var withTwoIngredients = CreateRecipe(
            name: "recipe with multiple ingredients",
            duration: TimeSpan.FromMinutes(10), 
            ingredients: [
                new RecipeIngredientData(ingredients.Water.Id, oneHundredMl, MeasurementUnit.Ml),
                new RecipeIngredientData(ingredients.Lemon.Id, onePiece, MeasurementUnit.Pieces)
            ]);

        var withDuplicateIngredientEntries = CreateRecipe(
            name: "recipe with lemon used twice",
            duration: TimeSpan.FromMinutes(30),
            type: RecipeType.Soup,
            ingredients: [
                new RecipeIngredientData(ingredients.Water.Id, fiveHundredMl, MeasurementUnit.Ml),
                new RecipeIngredientData(ingredients.Lemon.Id, oneHundredMl, MeasurementUnit.Ml),
                new RecipeIngredientData(ingredients.Lemon.Id, onePiece, MeasurementUnit.Pieces)
            ]);

        List<RecipeIngredientData> maximumIngredients = [];
        for (var index = 0; index < Recipe.MaxIngredients; index++)
        {
            maximumIngredients.Add(new RecipeIngredientData(ingredients.Water.Id, oneHundredMl, MeasurementUnit.Ml));
        }

        var withMaximumIngredients = CreateRecipe(
            name: "recipe with 10 ingredients",
            duration: TimeSpan.FromMinutes(120),
            type: RecipeType.Soup,
            ingredients: maximumIngredients);

        IReadOnlyList<Recipe> all = [withSingleIngredient, withTwoIngredients, withDuplicateIngredientEntries, withMaximumIngredients];

        for (var i = 0; i < all.Count; i++)
        {
            all[i].CreatedAt = new DateTime(2025, 1, i + 1, 0, 0, 0, DateTimeKind.Utc);
            all[i].ModifiedAt = new DateTime(2025, 1, all.Count - i, 0, 0, 0, DateTimeKind.Utc);
        }

        return new RecipeTestDataSet(
            withSingleIngredient,
            withTwoIngredients,
            withDuplicateIngredientEntries,
            withMaximumIngredients,
            all);
    }
}

public sealed record RecipeTestDataSet(
    Recipe WithSingleIngredient,
    Recipe WithTwoIngredients,
    Recipe WithDuplicateIngredientEntries,
    Recipe WithMaximumIngredients,
    IReadOnlyList<Recipe> All);
