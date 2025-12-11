namespace CookBook.Clean.WebApi.DTOs;

public class IngredientUpdateRequestDto
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Descripton { get; set; } = null;
    public string? ImageUrl { get; set; } = null;
}