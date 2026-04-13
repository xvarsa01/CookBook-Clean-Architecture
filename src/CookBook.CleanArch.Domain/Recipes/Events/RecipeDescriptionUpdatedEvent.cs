using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.Events;

public class RecipeDescriptionUpdatedEvent : IDomainEvent
{
    public RecipeDescriptionUpdatedEvent(RecipeId recipeId, string? oldDescription, string newDescription)
    {
        RecipeId = recipeId;
        OldDescription = oldDescription;
        NewDescription = newDescription;
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public RecipeId RecipeId { get; }
    public string? OldDescription { get; }
    public string NewDescription { get; }
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
}
