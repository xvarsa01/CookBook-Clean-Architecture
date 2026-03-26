using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Ingredient;

public record IngredientDetailResponse(
    IngredientId Id,
    string Name,
    string? Description,
    ImageUrl? ImageUrl
);
