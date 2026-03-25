using CookBook.CleanArch.Domain.RecipeRoot.Enums;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeAddIngredientRequest(
    Guid IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit
);
