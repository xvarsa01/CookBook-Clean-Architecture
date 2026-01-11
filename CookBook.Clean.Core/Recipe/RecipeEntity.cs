using CookBook.Clean.Core.Ingredient;

namespace CookBook.Clean.Core.Recipe;

public class RecipeEntity(string name, string? description, string? imageUrl) : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public string? ImageUrl { get; private set; } = imageUrl;
    public RecipeType Type { get; set; }
    

    private readonly List<IngredientInRecipeEntity> _ingredients = [];
    public IReadOnlyCollection<IngredientInRecipeEntity> Ingredients => _ingredients.AsReadOnly();
    
    public Guid AddIngredient(Guid ingredientId, decimal amount, MeasurementUnit unit)
    {
        var ingredientInRecipeId = Guid.NewGuid();
        var ingredient = new IngredientInRecipeEntity(ingredientInRecipeId, ingredientId, amount, unit);
        _ingredients.Add(ingredient);
        return ingredientInRecipeId;
    }

    public void RemoveIngredientEntry(Guid entryId)
    {
        var idx = _ingredients.FindIndex(i => i.Id == entryId);
        if (idx >= 0) _ingredients.RemoveAt(idx);
    }

    public RecipeEntity UpdateName(string newName)
    {
        if (Name == newName) return this;
        Name = newName;
        return this;
    }
    
    public RecipeEntity UpdateDescription(string newDescription)
    {
        if (Description == newDescription) return this;
        Description = newDescription;
        return this;
    }
    
    public RecipeEntity UpdateImageUrl(string newUrl)
    {
        if (ImageUrl == newUrl) return this;
        ImageUrl = newUrl;
        return this;
    }
}
