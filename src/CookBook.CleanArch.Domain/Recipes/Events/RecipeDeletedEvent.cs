using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.Events;

public record RecipeDeletedEvent(Guid recipeId) : IDomainEvent
{
    public Guid RecipeId { get; } = recipeId;
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
}
