using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Core.RecipeRoot;

// business rules:
// - recipe must have a name
//   - name must be minimal 3 characters long
// - recipe duration must be positive
// - recipe can have 0-10 ingredients
//   - ingredient amount must be positive

public class RecipeEntity : IRootEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RecipeName Name { get; private set; }
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; }
    public RecipeDuration Duration { get; private set; }
    public RecipeType Type { get; private set; }
    

    private readonly List<IngredientInRecipeEntity> _ingredients = [];

    public RecipeEntity(RecipeName name, string? description, string? imageUrl, RecipeDuration duration, RecipeType type)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Duration = duration;
        Type = type;
    }

    public IReadOnlyCollection<IngredientInRecipeEntity> Ingredients => _ingredients.AsReadOnly();
    
    public Guid AddIngredient(Guid ingredientId, IngredientAmount amount, MeasurementUnit unit)
    {
        if (_ingredients.Count == 10)
            throw new ArgumentException("Recipe cannot have more than 10 ingredients.");
        
        var ingredientInRecipeId = Guid.NewGuid();
        var ingredient = new IngredientInRecipeEntity(ingredientInRecipeId, ingredientId, amount, unit);
        _ingredients.Add(ingredient);
        return ingredientInRecipeId;
    }

    public void RemoveIngredientsByIngredientId(Guid ingredientId)
    {
        var removedCount = _ingredients.RemoveAll(i => i.IngredientId == ingredientId);

        if (removedCount == 0)
            throw new InvalidOperationException($"Ingredient {ingredientId} not found in recipe {Id}.");
    }
    
    public void RemoveIngredientByEntryId(Guid entryId)
    {
        var idx = _ingredients.FindIndex(i => i.Id == entryId);

        if (idx < 0)
        {
            throw new InvalidOperationException($"Ingredient entry {entryId} not found.");
        }
        _ingredients.RemoveAt(idx);
    }
    
    public void RemoveAllIngredients()
    {
        if (_ingredients.Count == 0)
        {
            throw new InvalidOperationException($"Recipe entity {Id} has no ingredients.");
        }
        _ingredients.Clear();
    }

    public void UpdateIngredientEntry(Guid entryId, IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == entryId);

        if (ingredient is null)
            throw new InvalidOperationException($"Ingredient entry {entryId} not found.");

        ingredient.Update(newAmount, newUnit);
    }

    public void UpdateName(RecipeName newName)
    {
        if (Name == newName) return;
        // fire some event?
        Name = newName;
    }
    
    public void UpdateDescription(string newDescription)
    {
        if (Description == newDescription) return;
        Description = newDescription;
    }
    
    public void UpdateRest(string? newUrl, RecipeDuration? newDuration, RecipeType? newType)
    {
        if (ImageUrl != newUrl)
        {
            ImageUrl = newUrl;
        }

        if (newDuration is not null && Duration != newDuration)
        {
            Duration = newDuration;
        }
        
        if (newType is not null && Type != newType)
        {
            Type = newType.Value;
        }
    }
}
