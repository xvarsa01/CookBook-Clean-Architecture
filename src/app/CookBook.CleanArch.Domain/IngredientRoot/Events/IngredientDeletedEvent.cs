using MediatR;

namespace CookBook.CleanArch.Domain.IngredientRoot.Events;

public class IngredientDeletedEvent(Guid ingredientId) : INotification
{
    public Guid IngredientId { get; } = ingredientId;
}
