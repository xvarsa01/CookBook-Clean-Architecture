using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Ingredient;

public record IngredientGetListResponse(
    Guid Id,
    string Name,
    ImageUrl? ImageUrl
);
