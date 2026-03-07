using CookBook.Clean.Core.Ingredient;

namespace CookBook.Clean.Core.Recipe;

public class RecipeEntity(string name, string? description, string? imageUrl, TimeSpan duration, RecipeType type) : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public string? ImageUrl { get; private set; } = imageUrl;
    public TimeSpan Duration { get; private set; } = duration;
    public RecipeType Type { get; private set; } = type;
    

    private readonly List<IngredientInRecipeEntity> _ingredients = [];
    public IReadOnlyCollection<IngredientInRecipeEntity> Ingredients => _ingredients.AsReadOnly();
    
    public Guid AddIngredient(Guid ingredientId, decimal amount, MeasurementUnit unit)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        
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

    public void UpdateIngredientEntry(Guid entryId, decimal newAmount, MeasurementUnit newUnit)
    {
        if (newAmount <= 0)
            throw new ArgumentException  ("Amount must be positive");
        
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == entryId);

        if (ingredient is null)
            throw new InvalidOperationException($"Ingredient entry {entryId} not found.");

        ingredient.Update(newAmount, newUnit);
    }

    public RecipeEntity UpdateName(string newName)
    {
        if (Name == newName) return this;
        // fire some event?
        Name = newName;
        return this;
    }
    
    public RecipeEntity UpdateDescription(string newDescription)
    {
        if (Description == newDescription) return this;
        Description = newDescription;
        return this;
    }
    
    public RecipeEntity UpdateRest(string? newUrl, TimeSpan? newDuration, RecipeType? newType)
    {
        if (ImageUrl != newUrl)
        {
            ImageUrl = newUrl;
        }

        if (newDuration is not null && Duration != newDuration)
        {
            Duration = newDuration.Value;
        }
        
        if (newType is not null && Type != newType)
        {
            Type = newType.Value;
        }

        return this;
    }


}
