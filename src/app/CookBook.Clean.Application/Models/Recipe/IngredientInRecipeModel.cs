using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models;

public record IngredientInRecipeModel : IModel
{
    public required Guid Id { get; set; }
    public required Guid IngredientId { get; set; }
    public required IngredientAmount Amount { get; set; }
    public required MeasurementUnit Unit { get; set; }
    public required string Name { get; set; }
    public ImageUrl? ImageUrl { get; set; }
}
