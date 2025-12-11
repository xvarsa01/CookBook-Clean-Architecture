using CookBook.Clean.Core.Recipe;

namespace CookBook.Clean.UseCases.Recipe;

public record RecipeIngredientModel
{
    public required Guid Id { get; set; }
    public required Guid IngredientId { get; set; }
    public required decimal Amount { get; set; }
    public required MeasurementUnit Unit { get; set; }
}
