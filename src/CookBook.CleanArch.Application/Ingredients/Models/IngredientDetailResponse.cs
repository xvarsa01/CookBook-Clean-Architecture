using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Models;

public record IngredientDetailResponse(
    IngredientId Id,
    string Name,
    string? Description,
    ImageUrl? ImageUrl
);
