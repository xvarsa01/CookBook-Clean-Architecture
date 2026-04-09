using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Models;

public record RecipeAddIngredientRequest(
    IngredientId IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit
);
