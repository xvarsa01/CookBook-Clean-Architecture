using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Models;

public record RecipeUpdateIngredientRequest(
    RecipeIngredientId EntryId,
    IngredientAmount NewAmount,
    MeasurementUnit NewUnit
);
