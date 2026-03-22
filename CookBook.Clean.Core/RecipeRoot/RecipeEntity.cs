using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.Exceptions;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Core.RecipeRoot;

// business rules:
// - recipe must have a name
//   - name must be minimal 3 characters long
// - recipe duration must be positive
// - recipe can have 0-10 ingredients
//   - ingredient amount must be positive

public class RecipeEntity(RecipeName name, string? description, string? imageUrl, RecipeDuration duration, RecipeType type)
    : IRootEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RecipeName Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public string? ImageUrl { get; private set; } = imageUrl;
    public RecipeDuration Duration { get; private set; } = duration;
    public RecipeType Type { get; private set; } = type;

    private readonly List<IngredientInRecipeEntity> _ingredients = [];

    public IReadOnlyCollection<IngredientInRecipeEntity> Ingredients => _ingredients.AsReadOnly();
    
    public Guid AddIngredient(Guid ingredientId, IngredientAmount amount, MeasurementUnit unit)
    {
        if (_ingredients.Count == 10)
            throw new RecipeMaximumNumberOfIngredients();
        
        var ingredientInRecipeId = Guid.NewGuid();
        var ingredient = new IngredientInRecipeEntity(ingredientInRecipeId, ingredientId, amount, unit);
        _ingredients.Add(ingredient);
        return ingredientInRecipeId;
    }

    public void RemoveIngredientsByIngredientId(Guid ingredientId)
    {
        var removedCount = _ingredients.RemoveAll(i => i.IngredientId == ingredientId);

        if (removedCount == 0)
            throw new RecipeIngredientByIdNotFoundException(ingredientId, Id);
    }
    
    public void RemoveIngredientByEntryId(Guid entryId)
    {
        var idx = _ingredients.FindIndex(i => i.Id == entryId);

        if (idx < 0)
            throw new RecipeIngredientByEntryIdNotFoundException(entryId, Id);
        
        _ingredients.RemoveAt(idx);
    }
    
    public void RemoveAllIngredients()
    {
        if (_ingredients.Count == 0)
            throw new RecipeHasNoIngredientsException(Id);
        
        _ingredients.Clear();
    }

    public void UpdateIngredientEntry(Guid entryId, IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == entryId);

        if (ingredient is null)
            throw new RecipeIngredientByEntryIdNotFoundException(entryId, Id);

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
