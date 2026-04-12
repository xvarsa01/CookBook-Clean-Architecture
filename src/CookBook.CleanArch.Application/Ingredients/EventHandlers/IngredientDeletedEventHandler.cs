using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Ingredient.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Application.Ingredients.EventHandlers;

public sealed class IngredientDeletedEventHandler(ILogger<IngredientDeletedEventHandler> logger, IEmailSender emailSender)
    : INotificationHandler<IngredientDeletedEvent>
{
    public async Task Handle(IngredientDeletedEvent notification, CancellationToken cancellationToken)
    {
        await emailSender.SendEmailAsync("to@test.com", "from@test.com",
            $"Ingredient {notification.IngredientId} was deleted",
            $"Ingredient {notification.IngredientId} was deleted.");
    }
}
