using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.WebApi.DTOs.Ingredient;

public record IngredientGetListDtoOut
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
};
