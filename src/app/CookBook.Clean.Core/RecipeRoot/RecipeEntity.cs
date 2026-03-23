using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.Exceptions;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Core.RecipeRoot;

// business rules:
// - recipe must have a name
//   - name must be minimal 3 characters long
// - recipe duration must be positive
// - recipe can have 0-10 ingredients
//   - ingredient amount must be positive

public class RecipeEntity(RecipeName name, string? description, ImageUrl? imageUrl, RecipeDuration duration, RecipeType type)
    : IRootEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RecipeName Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public ImageUrl? ImageUrl { get; private set; } = imageUrl;
    public RecipeDuration Duration { get; private set; } = duration;
    public RecipeType Type { get; private set; } = type;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; private set; }

    private readonly List<IngredientInRecipeEntity> _ingredients = [];

    public IReadOnlyCollection<IngredientInRecipeEntity> Ingredients => _ingredients.AsReadOnly();
    
    public Guid AddIngredient(Guid ingredientId, IngredientAmount amount, MeasurementUnit unit)
    {
        if (_ingredients.Count == 10)
            throw new RecipeMaximumNumberOfIngredients();
        
        var ingredientInRecipeId = Guid.NewGuid();
        var ingredient = new IngredientInRecipeEntity(ingredientInRecipeId, ingredientId, amount, unit);
        _ingredients.Add(ingredient);
        
        ModifiedAt = DateTime.UtcNow;
        return ingredientInRecipeId;
    }

    public void RemoveIngredientsByIngredientId(Guid ingredientId)
    {
        var removedCount = _ingredients.RemoveAll(i => i.IngredientId == ingredientId);

        if (removedCount == 0)
            throw new RecipeIngredientByIdNotFoundException(ingredientId, Id);
        
        ModifiedAt = DateTime.UtcNow;
    }
    
    public void RemoveIngredientByEntryId(Guid entryId)
    {
        var idx = _ingredients.FindIndex(i => i.Id == entryId);

        if (idx < 0)
            throw new RecipeIngredientByEntryIdNotFoundException(entryId, Id);
        
        _ingredients.RemoveAt(idx);
        ModifiedAt = DateTime.UtcNow;
    }
    
    public void RemoveAllIngredients()
    {
        if (_ingredients.Count == 0)
            throw new RecipeHasNoIngredientsException(Id);
        
        _ingredients.Clear();
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateIngredientEntry(Guid entryId, IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == entryId);

        if (ingredient is null)
            throw new RecipeIngredientByEntryIdNotFoundException(entryId, Id);

        ingredient.Update(newAmount, newUnit);
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateName(RecipeName newName)
    {
        if (Name == newName) return;
        
        // fire some event?
        Name = newName;
        ModifiedAt = DateTime.UtcNow;
    }
    
    public void UpdateDescription(string newDescription)
    {
        if (Description == newDescription) return;
        
        Description = newDescription;
        ModifiedAt = DateTime.UtcNow;
    }
    
    public void UpdateRest(ImageUrl? newUrl, RecipeDuration? newDuration, RecipeType? newType)
    {
        var updated = false;

        if (ImageUrl != newUrl)
        {
            ImageUrl = newUrl;
            updated = true;
        }

        if (newDuration is not null && Duration != newDuration)
        {
            Duration = newDuration;
            updated = true;
        }
    
        if (newType is not null && Type != newType)
        {
            Type = newType.Value;
            updated = true;
        }

        if (updated)
        {
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
