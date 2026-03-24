using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.WebApi.DTOs;

public class RecipeCreateDtoOut
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public TimeSpan Duration { get; set; }
    public RecipeType Type { get; set; }
}
