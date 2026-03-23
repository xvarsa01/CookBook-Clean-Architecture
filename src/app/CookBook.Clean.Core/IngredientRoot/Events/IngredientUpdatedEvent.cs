using MediatR;

namespace CookBook.Clean.Core.IngredientRoot.Events;

public class IngredientUpdatedEvent(IngredientEntity ingredient) : INotification
{
    public IngredientEntity Ingredient { get; } = ingredient;
}
