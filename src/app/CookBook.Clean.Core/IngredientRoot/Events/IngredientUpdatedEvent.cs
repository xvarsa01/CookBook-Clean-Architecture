using MediatR;

namespace CookBook.Clean.Core.IngredientRoot.Events;

public class IngredientUpdatedEvent(IngredientBase ingredient) : INotification
{
    public IngredientBase Ingredient { get; } = ingredient;
}
