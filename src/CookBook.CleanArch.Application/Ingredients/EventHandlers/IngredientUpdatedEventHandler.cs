using CookBook.CleanArch.Domain.Ingredient.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Application.Ingredients.EventHandlers;

public sealed class IngredientUpdatedEventHandler(ILogger<IngredientUpdatedEventHandler> logger)
    : INotificationHandler<IngredientNameUpdatedEvent>
{
    public Task Handle(IngredientNameUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Ingredient updated. Id={notification.IngredientId}, OldName={notification.OldName}, NewName={notification.NewName}");

        return Task.CompletedTask;
    }
}
