using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

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
