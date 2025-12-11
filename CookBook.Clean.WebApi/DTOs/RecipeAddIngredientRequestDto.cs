using CookBook.Clean.Core.Recipe;

namespace CookBook.Clean.WebApi.DTOs;

public class RecipeAddIngredientRequestDto
{
    public required Guid IngredientId { get; set; }
    public required decimal Amount { get; set; }
    public required MeasurementUnit Unit { get; set; }
}