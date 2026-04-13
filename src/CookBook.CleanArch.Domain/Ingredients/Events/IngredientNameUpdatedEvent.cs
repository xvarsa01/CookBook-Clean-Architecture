using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Ingredients.Events;

public class IngredientNameUpdatedEvent : IDomainEvent
{
    public IngredientNameUpdatedEvent(IngredientId ingredientId, string oldName, string newName)
    {
        IngredientId = ingredientId;
        OldName = oldName;
        NewName = newName;
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public IngredientId IngredientId { get; }
    public string OldName { get; }
    public string NewName { get; }
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
}
