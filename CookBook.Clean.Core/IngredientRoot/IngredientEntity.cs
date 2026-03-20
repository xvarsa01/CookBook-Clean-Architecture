namespace CookBook.Clean.Core.IngredientRoot;

public class IngredientEntity(string name, string? description, string? imageUrl) : IRootEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public string? ImageUrl { get; private set; } = imageUrl;
    
    public IngredientEntity UpdateName(string newName)
    {
        if (Name == newName) return this;
        Name = newName;
        return this;
    }
    
    public IngredientEntity UpdateDescription(string newDescription)
    {
        if (Description == newDescription) return this;
        Description = newDescription;
        return this;
    }
    
    public IngredientEntity UpdateImageUrl(string newUrl)
    {
        if (ImageUrl == newUrl) return this;
        ImageUrl = newUrl;
        return this;
    }
}
