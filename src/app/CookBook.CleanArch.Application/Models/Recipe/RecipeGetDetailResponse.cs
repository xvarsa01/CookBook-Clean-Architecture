using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeGetDetailResponse(
    Guid Id,
    RecipeName Name,
    string? Description,
    ImageUrl? ImageUrl,
    RecipeDuration Duration,
    RecipeType Type,
    ICollection<IngredientInRecipe> Ingredients
);

public record IngredientInRecipe(
    Guid Id,
    Guid IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit,
    string Name,
    ImageUrl? ImageUrl
) : IModel;
