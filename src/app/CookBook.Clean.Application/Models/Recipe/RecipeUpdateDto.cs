using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Recipe;

public record RecipeUpdateDto
{
    public required Guid Id { get; set; }
    public RecipeName? Name { get; set; }
    public string? Description { get; set; }
    public ImageUrl? ImageUrl { get; set; }
    public RecipeDuration? Duration { get; set; }
    public RecipeType? Type { get; set; }
}
