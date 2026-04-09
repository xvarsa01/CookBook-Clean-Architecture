using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Ingredient.Errors;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using MediatR;
using Moq;

namespace CookBook.CleanArch.UnitTests.UseCases.Ingredients;

public class IngredientUnitTests
{
    [Fact]
    public async Task CreateIngredientHandler_InsertsEntityAndReturnsId()
    {
        // Arrange
        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        
        var expectedId = new IngredientId(Guid.NewGuid());
        repoMock.Setup(r => r.InsertAsync(It.IsAny<Ingredient>()))
            .ReturnsAsync(expectedId);

        var handler = new CreateIngredientCommandHandler(repoMock.Object);
        var dto = new IngredientCreateRequest(
            "Sugar",
            "Sweet",
            ImageUrl.CreateObject("http://a.png").Value);
        var useCase = new CreateIngredientCommand(dto);

        // Act
        var result = await handler.Handle(useCase, CancellationToken.None);

        // Assert
        Assert.Equal(expectedId, result.Value);
        repoMock.Verify(r => r.InsertAsync(It.Is<Ingredient>(e => e.Name == "Sugar" && e.ImageUrl != null && e.ImageUrl.Value == "http://a.png")), Times.Once);
    }

    [Fact]
    public async Task UpdateIngredientHandler_ReturnsNotFound_WhenMissing()
    {
        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<IngredientId>())).ReturnsAsync((Ingredient?)null);

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdateIngredientCommandHandler(repoMock.Object, publisherMock.Object);
        var id = new IngredientId(Guid.NewGuid());
        var dto = new IngredientUpdateRequest(
            id,
            "New",
            "NewDesc",
            ImageUrl.CreateObject("http://a.png").Value);
        var useCase = new UpdateIngredientCommand(dto);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(
            IngredientErrors.IngredientNotFoundError(new IngredientId(id)),
            result.Error);
    }
}
