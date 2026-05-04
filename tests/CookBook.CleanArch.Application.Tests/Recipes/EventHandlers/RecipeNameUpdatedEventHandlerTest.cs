using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.EventHandlers;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Moq;

namespace CookBook.CleanArch.Application.Tests.Recipes.EventHandlers;

public class RecipeNameUpdatedEventHandlerTest
{
    [Fact]
    public async Task Handle_Should_Call_SendEmailAsync()
    {
        // Arrange
        var emailSenderMock = new Mock<IEmailSender>();
        var handler = new RecipeNameUpdatedEventHandler(emailSenderMock.Object);
        var recipeId = new RecipeId(Guid.NewGuid());
        var oldName = "Old Name";
        var newName = "New Name";
        var notification = new RecipeNameUpdatedEvent(recipeId, oldName, newName);

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        emailSenderMock.Verify(
            sender => sender.SendEmailAsync(
                "to@test.com",
                "from@test.com",
                $"Recipe {notification.RecipeId} Name Updated",
                $"Recipe with id {notification.RecipeId} has been updated from {notification.OldName} to {notification.NewName}. "
            ),
            Times.Once
        );
    }
}
