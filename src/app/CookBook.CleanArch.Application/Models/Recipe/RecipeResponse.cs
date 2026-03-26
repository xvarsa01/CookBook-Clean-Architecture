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
    IngredientInRecipeId Id,
    Guid IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit,
    string Name,
    ImageUrl? ImageUrl
);
