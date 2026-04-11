using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Models;

public record RecipeCreateRequest(
    RecipeName Name,
    string? Description,
    ImageUrl? ImageUrl,
    RecipeDuration Duration,
    RecipeType Type,
    IReadOnlyCollection<RecipeCreateIngredientRequest>? Ingredients = null
);

public sealed record RecipeCreateIngredientRequest(
    IngredientId IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit
);
