using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
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
