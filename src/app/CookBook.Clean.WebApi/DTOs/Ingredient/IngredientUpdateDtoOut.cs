namespace CookBook.Clean.WebApi.DTOs.Ingredient;

public class IngredientUpdateDtoOut
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
    public string? ImageUrl { get; set; } = null;
}
