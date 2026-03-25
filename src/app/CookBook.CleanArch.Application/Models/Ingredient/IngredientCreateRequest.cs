using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Ingredient;

public record IngredientCreateRequest(
    string Name,
    string? Description,
    ImageUrl? ImageUrl
);
