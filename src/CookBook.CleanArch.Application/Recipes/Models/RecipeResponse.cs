using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Models;

public record RecipeResponse(
    RecipeId Id,
    RecipeName Name,
    string? Description,
    ImageUrl? ImageUrl,
    RecipeDuration Duration,
    RecipeType Type,
    ICollection<RecipeIngredientResponse> Ingredients
);

public record RecipeIngredientResponse(
    RecipeIngredientId Id,
    IngredientId IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit,
    string IngredientName,
    ImageUrl? IngredientImageUrl
);
