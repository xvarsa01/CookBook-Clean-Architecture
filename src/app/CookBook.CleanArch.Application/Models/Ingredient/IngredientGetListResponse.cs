using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Ingredient;

public record IngredientGetListResponse(
    IngredientId Id,
    string Name,
    ImageUrl? ImageUrl
);
