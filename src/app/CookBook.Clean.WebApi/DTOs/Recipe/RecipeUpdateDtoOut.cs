using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.WebApi.DTOs;

public class RecipeUpdateDtoOut
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public TimeSpan? Duration { get; set; }
    public RecipeType? Type { get; set; }
}
