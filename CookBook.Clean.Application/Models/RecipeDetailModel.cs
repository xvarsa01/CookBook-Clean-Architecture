using System.Collections.ObjectModel;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Models;

public record RecipeDetailModel : IModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public required TimeSpan Duration { get; set; }
    public required RecipeType Type { get; set; }
    public ObservableCollection<IngredientInRecipeModel> Ingredients { get; set; } = [];
    
    public static RecipeDetailModel Empty
        => new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Duration = TimeSpan.Zero,
            Type = RecipeType.None,
        };

}
