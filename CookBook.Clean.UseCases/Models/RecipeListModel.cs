using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.UseCases.Models;

public record RecipeListModel : ModelBase
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    
    public required TimeSpan Duration { get; set; }
    public required RecipeType RecipeType { get; set; }
    public string? ImageUrl { get; set; }
}
