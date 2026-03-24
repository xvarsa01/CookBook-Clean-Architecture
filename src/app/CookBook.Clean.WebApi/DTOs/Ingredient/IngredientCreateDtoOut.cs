namespace CookBook.Clean.WebApi.DTOs.Ingredient;

public class IngredientCreateDtoOut
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
    public string? ImageUrl { get; set; } = null;
}
