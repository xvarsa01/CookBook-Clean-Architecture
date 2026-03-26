using CookBook.CleanArch.Domain.Ingredient.Errors;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Domain.Ingredient;

// business rules:
// - name can not be empty string

public record Ingredient : AggregateRootBase<IngredientId>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ImageUrl? ImageUrl { get; private set; }

    private Ingredient() { } // for EF
    private Ingredient(IngredientId id, string name, string? description, ImageUrl? imageUrl)
    {
        Id = id;
        Name =  name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public static Result<Ingredient> Create(string name, string? description, ImageUrl? imageUrl)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Invalid<Ingredient>(IngredientErrors.IngredientNameEmptyError());

        var id = IngredientId.CreateObject().Value;
        var entity = new Ingredient(id, name, description, imageUrl);
        return Result.Ok(entity);
    }

    public Result UpdateName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            return Result.Invalid(IngredientErrors.IngredientNameEmptyError());
        
        if (Name != newName)
        {
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
    
    public Result UpdateImageUrl(ImageUrl newUrl)
    {
        if (ImageUrl != newUrl)
        {
            ImageUrl = newUrl;
        }
        return Result.Ok();
    }
}
