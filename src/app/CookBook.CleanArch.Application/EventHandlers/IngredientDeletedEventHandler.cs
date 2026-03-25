using CookBook.CleanArch.Domain.IngredientRoot.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Application.EventHandlers;

public sealed class IngredientDeletedEventHandler(ILogger<IngredientDeletedEventHandler> logger)
    : INotificationHandler<IngredientDeletedEvent>
{
    public Task Handle(IngredientDeletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Ingredient deleted. Id={IngredientId}", notification.IngredientId);
        return Task.CompletedTask;
    }
}
