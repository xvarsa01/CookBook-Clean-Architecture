using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.WebApi.DTOs.Recipe;

public record RecipeGetDetailDtoOut
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public required TimeSpan Duration { get; set; }
    public required RecipeType Type { get; set; }
    public required ICollection<IngredientInRecipeModel> Ingredients { get; set; }
}
