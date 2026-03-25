using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeAddIngredientRequest(
    Guid IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit
);
