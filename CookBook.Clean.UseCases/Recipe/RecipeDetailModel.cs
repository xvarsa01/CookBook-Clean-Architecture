using CookBook.Clean.Core.Recipe;

namespace CookBook.Clean.UseCases.Recipe;

public record RecipeDetailModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public RecipeType Type { get; set; }
    public List<IngredientInRecipeModel> Ingredients { get; set; } = new();
    
    public static RecipeDetailModel Empty
        => new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Type = RecipeType.MainDish
        };
}
