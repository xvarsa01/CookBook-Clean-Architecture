using MediatR;

namespace CookBook.CleanArch.Domain.Ingredient.Events;

public class IngredientUpdatedEvent(Ingredient ingredient) : INotification
{
    public Ingredient Ingredient { get; } = ingredient;
}
