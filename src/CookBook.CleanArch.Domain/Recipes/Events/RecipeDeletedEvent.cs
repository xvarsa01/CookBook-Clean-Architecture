using MediatR;

namespace CookBook.CleanArch.Domain.Recipes.Events;

public class RecipeDeletedEvent(Guid recipeId) : INotification
{
    public Guid RecipeId { get; } = recipeId;
}
