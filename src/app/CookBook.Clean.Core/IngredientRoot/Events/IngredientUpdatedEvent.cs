using MediatR;

namespace CookBook.Clean.Core.IngredientRoot.Events;

public class IngredientUpdatedEvent(Ingredient ingredient) : INotification
{
    public Ingredient Ingredient { get; } = ingredient;
}
