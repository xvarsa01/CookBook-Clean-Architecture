using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Models;

public record IngredientListResponse(
    IngredientId Id,
    string Name,
    ImageUrl? ImageUrl
);
