namespace CookBook.Clean.Application.Models;

public record IngredientListModel : IModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}
