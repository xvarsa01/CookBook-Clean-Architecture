using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Recipes;

public sealed record RecipeIngredientData(
    IngredientId IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit
);
