namespace CookBook.Clean.Core.IngredientRoot;

public class IngredientEntity(string name, string? description, string? imageUrl) : IRootEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public string? ImageUrl { get; private set; } = imageUrl;
    
    public void UpdateName(string newName)
    {
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
