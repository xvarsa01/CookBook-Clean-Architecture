using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Ingredient;

public record IngredientCreateRequest(
    string Name,
    string? Description,
    ImageUrl? ImageUrl
);
