using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Common.Tests;

public static class RecipeTestData
{
    public static Recipe CreateRecipe(IReadOnlyCollection<RecipeIngredientData>? ingredients = null)
    {
        ingredients ??=
        [
            new RecipeIngredientData(
                IngredientTestData.CreateIngredient().Id,
                IngredientAmount.CreateObject(1).Value,
                MeasurementUnit.Pieces)
        ];

        return Recipe.Create(
            name: RecipeName.CreateObject("recipe").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients: ingredients).Value;
    }
}
