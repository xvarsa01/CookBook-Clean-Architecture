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
    IReadOnlyCollection<RecipeUpdateWithIngredientsAddIngredientRequest>? Additions = null,
    IReadOnlyCollection<RecipeUpdateWithIngredientsUpdateIngredientRequest>? Updates = null,
    IReadOnlyCollection<RecipeIngredientId>? Removals = null
);

public sealed record RecipeUpdateWithIngredientsAddIngredientRequest(
    IngredientId IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit
);

public sealed record RecipeUpdateWithIngredientsUpdateIngredientRequest(
    RecipeIngredientId EntryId,
    IngredientAmount NewAmount,
    MeasurementUnit NewUnit
);






