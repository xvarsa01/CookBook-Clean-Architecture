using System.Collections.ObjectModel;
using CookBook.Clean.Core.Recipe;
using CookBook.Clean.UseCases.Models_MAUI;

namespace CookBook.Clean.UseCases.Recipe;

public record RecipeDetailModel : ModelBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public RecipeType Type { get; set; }
    public ObservableCollection<IngredientInRecipeModel> Ingredients { get; set; } = [];
    
    public static RecipeDetailModel Empty
        => new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Type = RecipeType.MainDish
        };
}
