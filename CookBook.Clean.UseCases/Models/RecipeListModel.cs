using CookBook.Clean.Core.Recipe;

namespace CookBook.Clean.UseCases.Models;

public record RecipeListModel : ModelBase
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    
    public required RecipeType RecipeType { get; set; }
    public string? ImageUrl { get; set; }
}
