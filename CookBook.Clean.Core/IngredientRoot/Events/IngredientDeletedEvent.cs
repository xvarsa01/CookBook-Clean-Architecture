using MediatR;

namespace CookBook.Clean.Core.IngredientRoot.Events;

public class IngredientDeletedEvent(Guid ingredientId) : INotification
{
    public Guid IngredientId { get; } = ingredientId;
}
