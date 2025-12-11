using CookBook.Clean.Core.Ingredient;

namespace CookBook.Clean.Core.Recipe;

public class RecipeEntity(string name, string? description, string? imageUrl) : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public string? ImageUrl { get; private set; } = imageUrl;
    public RecipeType Type { get; set; }
    
    private readonly List<Guid> _ingredientIds = [];
    public IReadOnlyCollection<Guid> IngredientIds => _ingredientIds.AsReadOnly();
    
    private readonly List<RecipeIngredient> _ingredients = [];
    public IReadOnlyCollection<RecipeIngredient> Ingredients => _ingredients.AsReadOnly();
    
    public void AddIngredient(Guid ingredientId)
    {
        _ingredientIds.Add(ingredientId);
    }

    public void AddIngredient(Guid ingredientId, decimal amount, MeasurementUnit unit)
    {
        _ingredients.Add(new RecipeIngredient(Guid.NewGuid(), ingredientId, amount, unit));
    }

    public void RemoveIngredientAt(int index)
    {
        if (index < 0 || index >= _ingredients.Count) return;
        _ingredients.RemoveAt(index);
    }

    public void RemoveIngredient(Guid ingredientId, decimal amount, MeasurementUnit unit)
    {
        var matchIndex = _ingredients.FindIndex(i => i.IngredientId == ingredientId && i.Amount == amount && i.Unit == unit);
        if (matchIndex >= 0)
        {
            _ingredients.RemoveAt(matchIndex);
        }
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

public record RecipeIngredient(Guid Id, Guid IngredientId, decimal Amount, MeasurementUnit Unit);
