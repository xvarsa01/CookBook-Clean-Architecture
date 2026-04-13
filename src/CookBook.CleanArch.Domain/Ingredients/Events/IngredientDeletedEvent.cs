using MediatR;

namespace CookBook.CleanArch.Domain.Ingredients.Events;

public class IngredientDeletedEvent(Guid ingredientId) : INotification
{
    public Guid IngredientId { get; } = ingredientId;
}
