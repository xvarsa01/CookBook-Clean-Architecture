using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Recipe;

public record RecipeCreateDto
{
    public required RecipeName Name { get; set; }
    public string? Description { get; set; }
    public ImageUrl? ImageUrl { get; set; }
    public required RecipeDuration Duration { get; set; }
    public required RecipeType Type { get; set; }
}
