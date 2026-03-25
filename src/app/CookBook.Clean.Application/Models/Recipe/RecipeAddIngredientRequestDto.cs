using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.Application.Models.Recipe;

public class RecipeAddIngredientRequestDto
{
    public required Guid IngredientId { get; set; }
    public required decimal Amount { get; set; }
    public required MeasurementUnit Unit { get; set; }
}
