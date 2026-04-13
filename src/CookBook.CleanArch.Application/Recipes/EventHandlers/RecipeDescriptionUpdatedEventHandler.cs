using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Recipes.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Application.Recipes.EventHandlers;

public sealed class RecipeDescriptionUpdatedEventHandler(ILogger<RecipeDescriptionUpdatedEventHandler> logger)
    : INotificationHandler<RecipeDescriptionUpdatedEvent>
{
    public Task Handle(RecipeDescriptionUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Recipe Description Updated for recipe {notification.RecipeId} with new description {notification.NewDescription}");
        return Task.CompletedTask;
    }
}
