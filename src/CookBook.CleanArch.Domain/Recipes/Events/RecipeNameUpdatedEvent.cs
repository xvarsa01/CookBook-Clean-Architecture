using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.Events;

public class RecipeNameUpdatedEvent : IDomainEvent
{
    public RecipeNameUpdatedEvent(RecipeId recipeId, string oldName, string newName)
    {
        RecipeId = recipeId;
        OldName = oldName;
        NewName = newName;
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public RecipeId RecipeId { get; }
    public string OldName { get; }
    public string NewName { get; }
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
}
