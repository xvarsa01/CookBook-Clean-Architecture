using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Models;

public record IngredientResponse(
    IngredientId Id,
    string Name,
    string? Description,
    ImageUrl? ImageUrl
);
