using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Recipes.Events;
using MediatR;

namespace CookBook.CleanArch.Application.Recipes.EventHandlers;

public sealed class RecipeNameUpdatedEventHandler(IEmailSender emailSender)
    : INotificationHandler<RecipeNameUpdatedEvent>
{
    public async Task Handle(RecipeNameUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await emailSender.SendEmailAsync("to@test.com", "from@test.com",
            $"Recipe {notification.RecipeId} Name Updated",
            $"Recipe with id {notification.RecipeId} has been updated from {notification.OldName} to {notification.NewName}. ");
    }
}
