namespace CookBook.Clean.Core.RecipeRoot;

public class RecipeEntity : IRootEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; }
    public TimeSpan Duration { get; private set; }
    public RecipeType Type { get; private set; }
    

    private readonly List<IngredientInRecipeEntity> _ingredients = [];

    public RecipeEntity(string name, string? description, string? imageUrl, TimeSpan duration, RecipeType type)
    {
        if (name.Length < 3)
        {
            throw new ArgumentException("Recipe name cannot shorter than 3 characters.");
        }

        if (duration.TotalMinutes <= 0)
        {
            throw new ArgumentException("Recipe duration must be positive.");
        }
        
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Duration = duration;
        Type = type;
    }

    public IReadOnlyCollection<IngredientInRecipeEntity> Ingredients => _ingredients.AsReadOnly();
    
    public Guid AddIngredient(Guid ingredientId, decimal amount, MeasurementUnit unit)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");

        if (Ingredients.Count == 10)
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

    public void UpdateIngredientEntry(Guid entryId, decimal newAmount, MeasurementUnit newUnit)
    {
        if (newAmount <= 0)
            throw new ArgumentException  ("Amount must be positive");
        
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == entryId);

        if (ingredient is null)
            throw new InvalidOperationException($"Ingredient entry {entryId} not found.");

        ingredient.Update(newAmount, newUnit);
    }

    public void UpdateName(string newName)
    {
        if (Name == newName) return;
        if (newName.Length < 3)
        {
            throw new ArgumentException("Recipe name cannot shorter than 3 characters.");
        }
        
        // fire some event?
        Name = newName;
    }
    
    public void UpdateDescription(string newDescription)
    {
        if (Description == newDescription) return;
        Description = newDescription;
    }
    
    public void UpdateRest(string? newUrl, TimeSpan? newDuration, RecipeType? newType)
    {
        if (newDuration.HasValue && newDuration.Value.TotalMinutes <= 0)
        {
            throw new ArgumentException("Recipe duration must be positive.");
        }
        
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
    }
}
