using MediatR;

namespace CookBook.CleanArch.Domain.IngredientRoot.Events;

public class IngredientUpdatedEvent(Ingredient ingredient) : INotification
{
    public Ingredient Ingredient { get; } = ingredient;
}
