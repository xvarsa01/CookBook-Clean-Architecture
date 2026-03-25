using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Ingredient;

public record IngredientGetDetailResponse(
    Guid Id,
    string Name,
    string? Description,
    ImageUrl? ImageUrl
);
