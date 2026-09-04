using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Models;

public record RecipeUpdateWithIngredientsRequest(
    RecipeId Id,
    RecipeName? Name,
    string? Description,
    ImageUrl? ImageUrl,
    RecipeDuration? Duration,
    RecipeType? Type,
    IReadOnlyCollection<RecipeUpdateIngredientRequest>? Ingredients = null
);

public sealed record RecipeUpdateIngredientRequest(
    IngredientId IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit
);

