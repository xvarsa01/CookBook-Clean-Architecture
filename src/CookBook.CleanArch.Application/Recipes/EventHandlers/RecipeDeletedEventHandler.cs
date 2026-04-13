using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Recipes.Events;
using MediatR;

namespace CookBook.CleanArch.Application.Recipes.EventHandlers;

public sealed class RecipeDeletedEventHandler(IEmailSender emailSender)
    : INotificationHandler<RecipeDeletedEvent>
{
    public async Task Handle(RecipeDeletedEvent notification, CancellationToken cancellationToken)
    {
        await emailSender.SendEmailAsync("to@test.com", "from@test.com",
            $"Recipe {notification.RecipeId} was deleted",
            $"Recipe {notification.RecipeId} was deleted.");
    }
}
