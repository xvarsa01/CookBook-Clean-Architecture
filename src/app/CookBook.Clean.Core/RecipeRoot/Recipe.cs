using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Core.RecipeRoot;

// business rules:
// - recipe must have a name
//   - name must be minimal 3 characters long
// - recipe duration must be positive
// - recipe can have 0-10 ingredients
//   - ingredient amount must be positive

public record Recipe : AggregateRootBase
{
    public override Guid Id { get; init; } = Guid.NewGuid();
    public RecipeName Name { get; private set; }
    public string? Description { get; private set; }
    public ImageUrl? ImageUrl { get; private set; }
    public RecipeDuration Duration { get; private set; }
    public RecipeType Type { get; private set; }

    private readonly List<IngredientInRecipeEntity> _ingredients = [];
    public IReadOnlyCollection<IngredientInRecipeEntity> Ingredients => _ingredients.AsReadOnly();
    
    private Recipe() { } // for EF
    private Recipe(RecipeName name, string? description, ImageUrl? imageUrl, RecipeDuration duration, RecipeType type)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Duration = duration;
        Type = type;
    }
    
    public static Result<Recipe> Create(RecipeName name, string? description, ImageUrl? imageUrl, RecipeDuration duration, RecipeType type)
    {
        var entity = new Recipe(name, description, imageUrl, duration, type);
        return Result.Ok(entity);
    }
    
    public Result<Guid> AddIngredient(Guid ingredientId, IngredientAmount amount, MeasurementUnit unit)
    {
        if (_ingredients.Count == 10)
            return Result.Invalid<Guid>("Recipe cannot have more than 10 ingredients.");
        
        var ingredientInRecipeResult = IngredientInRecipeEntity.Create(ingredientId, Id, amount, unit);
        
        if (ingredientInRecipeResult.IsFailure)
            return Result.Invalid<Guid>(ingredientInRecipeResult.Error);
        
        _ingredients.Add(ingredientInRecipeResult.Value);
        
        return Result.Ok(ingredientInRecipeResult.Value.Id);
    }

    public Result RemoveIngredientsByIngredientId(Guid ingredientId)
    {
        var removedCount = _ingredients.RemoveAll(i => i.IngredientId == ingredientId);

        if (removedCount == 0)
            return Result.Invalid($"Ingredient {ingredientId} not found in recipe {Id}.");
        
        return Result.Ok();
    }
    
    public Result RemoveIngredientByEntryId(Guid entryId)
    {
        var idx = _ingredients.FindIndex(i => i.Id == entryId);

        if (idx < 0)
            return Result.Invalid($"Ingredient entry for {entryId} not found in recipe {Id}.");
        
        _ingredients.RemoveAt(idx);
        return Result.Ok();
    }
    
    public Result RemoveAllIngredients()
    {
        if (_ingredients.Count == 0)
            return Result.Invalid($"Recipe entity {Id} has no ingredients.");
        
        _ingredients.Clear();
        return Result.Ok();
    }

    public Result UpdateIngredientEntry(Guid entryId, IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == entryId);

        if (ingredient is null)
            return Result.Invalid($"Ingredient entry for {entryId} not found in recipe {Id}.");

        ingredient.Update(newAmount, newUnit);
        return Result.Ok();
    }

    public Result UpdateName(RecipeName newName)
    {
        if (Name != newName)
        {
            // fire some event?
            Name = newName;
        }
        
        return Result.Ok();
    }
    
    public Result UpdateDescription(string newDescription)
    {
        if (Description != newDescription)
        {
            Description = newDescription;
        }
        
        return Result.Ok();
    }
    
    public Result UpdateRest(ImageUrl? newUrl, RecipeDuration? newDuration, RecipeType? newType)
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
        
        return Result.Ok();
    }
}
