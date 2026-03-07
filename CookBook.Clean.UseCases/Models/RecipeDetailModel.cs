using System.Collections.ObjectModel;
using CookBook.Clean.Core.Recipe;

namespace CookBook.Clean.UseCases.Models;

public record RecipeDetailModel : ModelBase
{
    public Guid Id { get; set; }
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
