namespace CookBook.Clean.UseCases.Recipe;

public record RecipeDetailModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<RecipeIngredientModel> Ingredients { get; set; } = new();
}
