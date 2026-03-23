using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Core.IngredientRoot;

// business rules:
// - name can not be empty string

public class IngredientEntity : IRootEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ImageUrl? ImageUrl { get; private set; }

    private IngredientEntity() { } // for EF
    private IngredientEntity(string name, string? description, ImageUrl? imageUrl)
    {
        Name =  name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public static Result<IngredientEntity> Create(string name, string? description, ImageUrl? imageUrl)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Invalid<IngredientEntity>("Ingredient name can not be empty.");

        var entity = new IngredientEntity(name, description, imageUrl);
        return Result.Ok(entity);
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new ArgumentException(nameof(newName));
        
        if (Name == newName) return;
        Name = newName;
    }
    
    public void UpdateDescription(string newDescription)
    {
        if (Description == newDescription) return;
        Description = newDescription;
    }
    
    public void UpdateImageUrl(ImageUrl newUrl)
    {
        if (ImageUrl == newUrl) return;
        ImageUrl = newUrl;
    }
}
