using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Ingredient;

public record IngredientGetListDto()
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public ImageUrl? ImageUrl { get; set; }
};
