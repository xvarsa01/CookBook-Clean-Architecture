using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Ingredient;

public record IngredientCreateDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ImageUrl? ImageUrl { get; set; }
};
