using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Models;

public record IngredientInRecipeModel : IModel
{
    public required Guid Id { get; set; }
    public required Guid IngredientId { get; set; }
    public required decimal Amount { get; set; }
    public required MeasurementUnit Unit { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}
