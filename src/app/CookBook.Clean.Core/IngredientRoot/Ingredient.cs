using CookBook.Clean.Core.IngredientRoot.ValueObjects;
using CookBook.Clean.Core.Shared;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Core.IngredientRoot;

// business rules:
// - name can not be empty string

public record Ingredient : AggregateRootBase<IngredientId>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ImageUrl? ImageUrl { get; private set; }

    private Ingredient() { } // for EF
    private Ingredient(string name, string? description, ImageUrl? imageUrl)
    {
        Id = new IngredientId(Guid.NewGuid());
        Name =  name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public static Result<Ingredient> Create(string name, string? description, ImageUrl? imageUrl)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Invalid<Ingredient>("Ingredient name can not be empty.");

        var entity = new Ingredient(name, description, imageUrl);
        return Result.Ok(entity);
    }

    public Result UpdateName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            return Result.Invalid("Ingredient name can not be empty.");
        
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
