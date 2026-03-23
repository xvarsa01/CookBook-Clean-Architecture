using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.Application.Models;

public record RecipeListModel : IModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
    
    public required TimeSpan? Duration { get; set; }
    public required RecipeType RecipeType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
