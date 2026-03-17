using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.WebApi.DTOs;

public class RecipeUpdateRequestDto
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public TimeSpan Duration { get; set; }
    public RecipeType Type { get; set; }
}
