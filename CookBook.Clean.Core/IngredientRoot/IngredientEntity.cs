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

    public IngredientEntity(string name, string? description, ImageUrl? imageUrl)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException(nameof(name));
        
        Name =  name;
        Description = description;
        ImageUrl = imageUrl;
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
