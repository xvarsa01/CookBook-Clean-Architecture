namespace CookBook.Clean.UseCases.Ingredient;

public record IngredientDetailModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}