using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Ingredient;

public record IngredientGetDetailDto()
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ImageUrl? ImageUrl { get; set; }
};
