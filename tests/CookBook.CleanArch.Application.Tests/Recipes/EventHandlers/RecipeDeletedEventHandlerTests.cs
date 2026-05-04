using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.EventHandlers;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Moq;

namespace CookBook.CleanArch.Application.Tests.Recipes.EventHandlers;

public class RecipeDeletedEventHandlerTests
{
    [Fact]
    public async Task Handle_Should_Call_SendEmailAsync()
    {
        // Arrange
        var emailSenderMock = new Mock<IEmailSender>();
        var handler = new RecipeDeletedEventHandler(emailSenderMock.Object);
        var recipeId = new RecipeId(Guid.NewGuid());
        var notification = new RecipeDeletedEvent(recipeId);

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        emailSenderMock.Verify(
            sender => sender.SendEmailAsync(
                "to@test.com",
                "from@test.com",
                $"Recipe {notification.RecipeId} was deleted",
                $"Recipe {notification.RecipeId} was deleted."
            ),
            Times.Once
        );
    }
}
