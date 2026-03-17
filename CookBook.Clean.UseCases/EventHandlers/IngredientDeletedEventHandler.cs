using CookBook.Clean.Core.IngredientRoot.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CookBook.Clean.UseCases.EventHandlers;

public sealed class IngredientDeletedEventHandler(ILogger<IngredientDeletedEventHandler> logger)
    : INotificationHandler<IngredientDeletedEvent>
{
    public Task Handle(IngredientDeletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Ingredient deleted. Id={IngredientId}", notification.IngredientId);
        return Task.CompletedTask;
    }
}
