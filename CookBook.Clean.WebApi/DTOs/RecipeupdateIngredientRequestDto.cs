using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.WebApi.DTOs;

public class RecipeUpdateIngredientRequestDto
{
    public required Guid EntryId { get; set; }
    public required decimal NewAmount { get; set; }
    public required MeasurementUnit NewUnit { get; set; }
}
