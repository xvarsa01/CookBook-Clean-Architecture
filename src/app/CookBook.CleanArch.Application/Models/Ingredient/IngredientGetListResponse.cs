using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Ingredient;

public record IngredientGetListResponse(
    Guid Id,
    string Name,
    ImageUrl? ImageUrl
);
