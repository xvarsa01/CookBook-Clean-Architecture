using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Models.Recipe;

public record RecipeAddIngredientRequest(
    Guid IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit
);
