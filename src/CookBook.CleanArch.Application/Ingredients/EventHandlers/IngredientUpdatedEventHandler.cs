using CookBook.CleanArch.Domain.Ingredient.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Application.Ingredients.EventHandlers;

public sealed class IngredientUpdatedEventHandler(ILogger<IngredientUpdatedEventHandler> logger)
    : INotificationHandler<IngredientUpdatedEvent>
{
    public Task Handle(IngredientUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Ingredient updated. Id={IngredientId}, Name={IngredientName}",
            notification.Ingredient.Id,
            notification.Ingredient.Name);

        return Task.CompletedTask;
    }
}
