namespace CookBook.Clean.WebApi.DTOs;

public class RecipeCreateRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
    public string? ImageUrl { get; set; } = null;
}
