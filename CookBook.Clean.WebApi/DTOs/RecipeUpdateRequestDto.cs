namespace CookBook.Clean.WebApi.DTOs;

public class RecipeUpdateRequestDto
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Descripton { get; set; }
    public string? ImageUrl { get; set; }
}
