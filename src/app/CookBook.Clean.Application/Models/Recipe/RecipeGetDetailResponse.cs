using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Recipe;

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
