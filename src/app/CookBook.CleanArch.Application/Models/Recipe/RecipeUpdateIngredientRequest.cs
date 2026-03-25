using CookBook.CleanArch.Domain.RecipeRoot.Enums;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeUpdateIngredientRequest(
    Guid EntryId,
    IngredientAmount NewAmount,
    MeasurementUnit NewUnit
);
