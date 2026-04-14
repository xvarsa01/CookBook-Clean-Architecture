using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Domain.Recipes;

// business rules:
// - recipe must have a name
//   - name must be minimal 3 characters long
// - recipe duration must be positive
// - recipe can have 1-10 ingredients
//   - ingredient amount must be positive

public record Recipe : AggregateRootBase<RecipeId>
{
    public RecipeName Name { get; private set; }
    public string? Description { get; private set; }
    public ImageUrl? ImageUrl { get; private set; }
    public RecipeDuration Duration { get; private set; }
    public RecipeType Type { get; private set; }

    private readonly List<RecipeIngredient> _ingredients = [];
    public IReadOnlyCollection<RecipeIngredient> Ingredients => _ingredients.AsReadOnly();
    
    private const int MaxIngredients = 10;
    
    private Recipe(RecipeId id, RecipeName name, string? description, ImageUrl? imageUrl, RecipeDuration duration, RecipeType type) : base(id)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Duration = duration;
        Type = type;
    }
    
    public static Result<Recipe> Create(
        RecipeName name,
        string? description,
        ImageUrl? imageUrl,
        RecipeDuration duration,
        RecipeType type,
        IReadOnlyCollection<RecipeCreateIngredient> ingredients)
    {
        var id = new RecipeId(Guid.NewGuid());

        if (ingredients.Count == 0)
            return Result.Invalid<Recipe>(RecipeErrors.RecipeMinimumNumberOfIngredientsError(id));

        var recipe = new Recipe(id, name, description, imageUrl, duration, type);

        foreach (var ingredient in ingredients)
        {
            var addResult = recipe.AddIngredient(ingredient.IngredientId, ingredient.Amount, ingredient.Unit);
            if (addResult.IsFailure)
            {
                return Result.Invalid<Recipe>(addResult.Error);
            }
        }

        return Result.Ok(recipe);
    }
    
    public Result<RecipeIngredientId> AddIngredient(IngredientId ingredientId, IngredientAmount amount, MeasurementUnit unit)
    {
        if (_ingredients.Count >= MaxIngredients)
            return Result.Invalid<RecipeIngredientId>(RecipeErrors.RecipeMaximumNumberOfIngredientsError(Id));
        
        var ingredientInRecipeResult = RecipeIngredient.Create(ingredientId, Id, amount, unit);
        
        if (ingredientInRecipeResult.IsFailure)
            return Result.Invalid<RecipeIngredientId>(ingredientInRecipeResult.Error);
        
        _ingredients.Add(ingredientInRecipeResult.Value);
        
        return Result.Ok(ingredientInRecipeResult.Value.Id);
    }

    public Result RemoveIngredientsByIngredientId(IngredientId ingredientId)
    {
        var removedCount = _ingredients.Count(i => i.IngredientId == ingredientId);

        if (removedCount == 0)
            return Result.Invalid(RecipeErrors.RecipeIngredientByIdNotFoundError(ingredientId, Id));

        if (_ingredients.Count - removedCount < 1)
            return Result.Invalid(RecipeErrors.RecipeMinimumNumberOfIngredientsError(Id));

        _ingredients.RemoveAll(i => i.IngredientId == ingredientId);
        
        return Result.Ok();
    }
    
    public Result RemoveIngredientByEntryId(RecipeIngredientId entryId)
    {
        var idx = _ingredients.FindIndex(i => i.Id == entryId);

        if (idx < 0)
            return Result.Invalid(RecipeErrors.RecipeIngredientByEntryIdNotFoundError(entryId, Id));

        if (_ingredients.Count == 1)
            return Result.Invalid(RecipeErrors.RecipeMinimumNumberOfIngredientsError(Id));
        
        _ingredients.RemoveAt(idx);
        return Result.Ok();
    }

    public Result UpdateIngredientEntry(RecipeIngredientId entryId, IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == entryId);

        if (ingredient is null)
            return Result.Invalid(RecipeErrors.RecipeIngredientByEntryIdNotFoundError(entryId, Id));

        ingredient.Update(newAmount, newUnit);
        return Result.Ok();
    }

    public Result UpdateName(RecipeName newName)
    {
        if (Name != newName)
        {
            RaiseEvent(new RecipeNameUpdatedEvent(Id, Name.Value, newName.Value));
            Name = newName;
        }
        
        return Result.Ok();
    }
    
    public Result UpdateDescription(string newDescription)
    {
        if (Description != newDescription)
        {
            RaiseEvent(new RecipeDescriptionUpdatedEvent(Id, Description, newDescription));
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

    public Result Delete()
    {
        RaiseEvent(new RecipeDeletedEvent(Id));
        return Result.Ok();
    }
}
