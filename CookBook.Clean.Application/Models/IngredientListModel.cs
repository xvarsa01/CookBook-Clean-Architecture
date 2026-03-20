namespace CookBook.Clean.Application.Models;

public record IngredientListModel : ModelBase
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}
