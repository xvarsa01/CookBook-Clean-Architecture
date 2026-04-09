using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Models;

public record IngredientUpdateRequest(
    IngredientId Id,
    string? Name,
    string? Description,
    ImageUrl? ImageUrl
);
