using MediatR;

namespace CookBook.Clean.Core.Ingredient.Events;

public class IngredientUpdatedEvent(IngredientEntity ingredient) : INotification
{
    public IngredientEntity Ingredient { get; } = ingredient;
}
