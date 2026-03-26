using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Ingredient;

public record IngredientGetDetailResponse(
    IngredientId Id,
    string Name,
    string? Description,
    ImageUrl? ImageUrl
);
