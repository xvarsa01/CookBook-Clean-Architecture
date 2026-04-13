using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Ingredients.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Application.Ingredients.EventHandlers;

public sealed class IngredientUpdatedEventHandler(ILogger<IngredientUpdatedEventHandler> logger, IEmailSender emailSender)
    : INotificationHandler<IngredientNameUpdatedEvent>
{
    public async Task Handle(IngredientNameUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await emailSender.SendEmailAsync("to@test.com", "from@test.com",
            $"Ingredient {notification.IngredientId} Name Updated",
            $"Ingredient with id {notification.IngredientId} has been updated from {notification.OldName} to {notification.NewName}. ");
    }
}
