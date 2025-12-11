namespace CookBook.Clean.UseCases.Recipe;

public record RecipeListModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}
