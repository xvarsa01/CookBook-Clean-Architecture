using CookBook.CleanArch.Application.Recipes.EventHandlers;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CookBook.CleanArch.Application.UnitTests.Recipes.EventHandlers;

public class RecipeDescriptionUpdatedEventHandlerTest
{
    [Fact]
    public async Task Handle_Should_LogInformation()
    {
        // Arrange
        var loggerMock = Substitute.For<ILogger<RecipeDescriptionUpdatedEventHandler>>();
        var handler = new RecipeDescriptionUpdatedEventHandler(loggerMock);
        var recipeId = new RecipeId(Guid.NewGuid());
        var oldDescription = "Old description";
        var newDescription = "New description";
        var notification = new RecipeDescriptionUpdatedEvent(recipeId, oldDescription, newDescription);

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        var logCall = Assert.Single(loggerMock.ReceivedCalls(),
            call => call.GetMethodInfo().Name == nameof(ILogger.Log));

        Assert.Equal(LogLevel.Information, logCall.GetArguments()[0]);
        Assert.Contains(
            $"Recipe Description Updated for recipe {notification.RecipeId} with new description {notification.NewDescription}",
            logCall.GetArguments()[2]?.ToString());
    }
}
