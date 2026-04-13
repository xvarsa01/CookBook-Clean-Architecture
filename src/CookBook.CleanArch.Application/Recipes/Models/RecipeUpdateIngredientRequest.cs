using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Models;

public record RecipeUpdateIngredientRequest(
    RecipeIngredientId EntryId,
    IngredientAmount NewAmount,
    MeasurementUnit NewUnit
);
