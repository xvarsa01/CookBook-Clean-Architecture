using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.WebApi.DTOs;

public class RecipeCreateRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public TimeSpan Duration { get; set; }
    public RecipeType Type { get; set; }
}
