using CookBook.CleanArch.Application.Recipes.EventHandlers;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace CookBook.CleanArch.Application.Tests.Recipes.EventHandlers;

public class RecipeDescriptionUpdatedEventHandlerTest
{
    [Fact]
    public Task Handle_Should_LogInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<RecipeDescriptionUpdatedEventHandler>>();
        var handler = new RecipeDescriptionUpdatedEventHandler(loggerMock.Object);
        var recipeId = new RecipeId(Guid.NewGuid());
        var oldDescription = "Old description";
        var newDescription = "New description";
        var notification = new RecipeDescriptionUpdatedEvent(recipeId, oldDescription, newDescription);

        // Act
        handler.Handle(notification, CancellationToken.None);

        // Assert
        loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Recipe Description Updated for recipe {notification.RecipeId} with new description {notification.NewDescription}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        
        return Task.CompletedTask;
    }
}
