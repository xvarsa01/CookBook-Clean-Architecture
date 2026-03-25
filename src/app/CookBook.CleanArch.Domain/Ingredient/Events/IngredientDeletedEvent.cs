using MediatR;

namespace CookBook.CleanArch.Domain.Ingredient.Events;

public class IngredientDeletedEvent(Guid ingredientId) : INotification
{
    public Guid IngredientId { get; } = ingredientId;
}
