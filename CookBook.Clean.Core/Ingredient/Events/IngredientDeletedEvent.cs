using MediatR;

namespace CookBook.Clean.Core.Ingredient.Events;

public class IngredientDeletedEvent(Guid ingredientId) : INotification
{
    public Guid IngredientId { get; } = ingredientId;
}
