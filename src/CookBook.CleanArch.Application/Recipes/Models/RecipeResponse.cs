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
    ICollection<RecipeIngredientResponse> Ingredients,
    ICollection<RecipeReviewResponse> Reviews,
    decimal? AverageMark
);

public record RecipeIngredientResponse(
    RecipeIngredientId Id,
    IngredientId IngredientId,
    IngredientAmount Amount,
    MeasurementUnit Unit,
    string IngredientName,
    ImageUrl? IngredientImageUrl
);

public record RecipeReviewResponse(
    RecipeReviewId Id,
    int Mark,
    string Description
);
