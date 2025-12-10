using CookBook.Clean.Core.Ingredient;

namespace CookBook.Clean.Core.Recipe;

public class RecipeEntity(string name, string? description, string? imageUrl) : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public string? ImageUrl { get; private set; } = imageUrl;
    
    private readonly List<Guid> _ingredientIds = [];
    public IReadOnlyCollection<Guid> IngredientIds => _ingredientIds.AsReadOnly();
    
    public void AddIngredient(Guid ingredientId)
    {
        _ingredientIds.Add(ingredientId);
    }
}