using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.WebApi.DTOs;

public class RecipeUpdateIngredientRequestDto
{
    public required Guid EntryId { get; set; }
    public required decimal NewAmount { get; set; }
    public required MeasurementUnit NewUnit { get; set; }
}
