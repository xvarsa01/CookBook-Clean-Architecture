using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.Events;

public record RecipeDeletedEvent : IDomainEvent
{
    public RecipeDeletedEvent(RecipeId recipeId)
    {
        RecipeId = recipeId;
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
    
    public RecipeId RecipeId { get; }
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
}
