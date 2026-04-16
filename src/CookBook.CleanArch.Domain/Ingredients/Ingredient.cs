using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Domain.Ingredients;

// business rules:
// - name can not be empty string

public record Ingredient : AggregateRootBase<IngredientId>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ImageUrl? ImageUrl { get; private set; }

    private Ingredient(IngredientId id, string name, string? description, ImageUrl? imageUrl) : base(id)
    {
        Name =  name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public static Result<Ingredient> Create(string name, string? description, ImageUrl? imageUrl)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Failure<Ingredient>(IngredientErrors.IngredientNameEmptyError());

        var id = new IngredientId(Guid.NewGuid());
        var entity = new Ingredient(id, name, description, imageUrl);
        return Result.Success(entity);
    }

    public Result UpdateName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            return Result.Failure(IngredientErrors.IngredientNameEmptyError());
        
        if (Name != newName)
        {
            Name = newName;
        }
        
        return Result.Success();
    }
    
    public Result UpdateDescription(string newDescription)
    {
        if (Description != newDescription)
        {
            Description = newDescription;
        }
        
        return Result.Success();
    }
    
    public Result UpdateImageUrl(ImageUrl newUrl)
    {
        if (ImageUrl != newUrl)
        {
            ImageUrl = newUrl;
        }
        return Result.Success();
    }
}
