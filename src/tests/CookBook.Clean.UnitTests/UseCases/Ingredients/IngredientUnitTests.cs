using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Errors;
using CookBook.Clean.Core.IngredientRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;
using MediatR;
using Moq;

namespace CookBook.Clean.UnitTests.UseCases.Ingredients;

public class IngredientUnitTests
{
    [Fact]
    public async Task CreateIngredientHandler_InsertsEntityAndReturnsId()
    {
        // Arrange
        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        
        var expectedId = Guid.NewGuid();
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
    public async Task GetIngredientHandler_ReturnsNotFound_WhenMissing()
    {
        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);

        var handler = new GetIngredientDetailQueryHandler(repoMock.Object);
        var useCase = new GetIngredientDetailQuery(Guid.NewGuid());

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(
            IngredientErrors.IngredientNotFoundError(new IngredientId(useCase.Id)),
            result.Error);
    }

    [Fact]
    public async Task GetIngredientHandler_ReturnsOk_WhenFound()
    {
        var entity = Ingredient.Create("Salt", null, ImageUrl.CreateObject("http://image.png").Value).Value;
        var id = entity.Id;


        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

        var handler = new GetIngredientDetailQueryHandler(repoMock.Object);
        var useCase = new GetIngredientDetailQuery(id);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal("Salt", result.Value.Name);
    }

    [Fact]
    public async Task UpdateIngredientHandler_ReturnsNotFound_WhenMissing()
    {
        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdateIngredientCommandHandler(repoMock.Object, publisherMock.Object);
        var id = Guid.NewGuid();
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

    [Fact]
    public async Task UpdateIngredientHandler_UpdatesAndReturnsOk()
    {
        var entity = Ingredient.Create("Old", "d", null).Value;
        var id = entity.Id;

        var repoMock = new Mock<IRepository<Ingredient, IngredientId>>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        repoMock.Setup(r => r.UpdateAsync(It.IsAny<Ingredient>())).ReturnsAsync(id);

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdateIngredientCommandHandler(repoMock.Object, publisherMock.Object);
        var dto = new IngredientUpdateRequest(
            id,
            "New",
            "NewDesc",
            ImageUrl.CreateObject("http://a.png").Value);
        var useCase = new UpdateIngredientCommand(dto);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsSuccess);
        repoMock.Verify(r => r.UpdateAsync(It.Is<Ingredient>(e => e.Name == "New" && e.Description == "NewDesc" && e.ImageUrl != null && e.ImageUrl.Value == "http://a.png")), Times.Once);
        publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
