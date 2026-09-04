using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.EventHandlers;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.UnitTests.Recipes.EventHandlers;

public class RecipeNameUpdatedEventHandlerTest
{
    [Fact]
    public async Task Handle_Should_Call_SendEmailAsync()
    {
        // Arrange
        var emailSenderMock = Substitute.For<IEmailSender>();
        var handler = new RecipeNameUpdatedEventHandler(emailSenderMock);
        var recipeId = new RecipeId(Guid.NewGuid());
        var oldName = "Old Name";
        var newName = "New Name";
        var notification = new RecipeNameUpdatedEvent(recipeId, oldName, newName);

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        await emailSenderMock.Received(1).SendEmailAsync(
            "to@test.com",
            "from@test.com",
            $"Recipe {notification.RecipeId} Name Updated",
            $"Recipe with id {notification.RecipeId} has been updated from {notification.OldName} to {notification.NewName}. ");
    }
}
