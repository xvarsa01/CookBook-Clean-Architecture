using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeUpdateIngredientRequest(
    IngredientInRecipeId EntryId,
    IngredientAmount NewAmount,
    MeasurementUnit NewUnit
);
