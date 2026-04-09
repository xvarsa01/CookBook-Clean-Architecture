using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Models;

public record IngredientCreateRequest(
    string Name,
    string? Description,
    ImageUrl? ImageUrl
);
