namespace CookBook.Clean.Core.IngredientRoot;

// business rules:
// - name can not be empty string

public class IngredientEntity : IRootEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; }

    public IngredientEntity(string name, string? description, string? imageUrl)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        
        Name =  name;
        Description = description;
        ImageUrl = imageUrl;
    }
    
    public void UpdateName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new ArgumentNullException(nameof(newName));
        
        if (Name == newName) return;
        Name = newName;
    }
    
    public void UpdateDescription(string newDescription)
    {
        if (Description == newDescription) return;
        Description = newDescription;
    }
    
    public void UpdateImageUrl(string newUrl)
    {
        if (ImageUrl == newUrl) return;
        ImageUrl = newUrl;
    }
}
