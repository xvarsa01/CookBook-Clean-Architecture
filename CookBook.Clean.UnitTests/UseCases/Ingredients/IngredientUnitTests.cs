using System.Diagnostics;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.UseCases;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.IngredientRoot.Create;
using CookBook.Clean.UseCases.IngredientRoot.Delete;
using CookBook.Clean.UseCases.IngredientRoot.Get;
using CookBook.Clean.UseCases.IngredientRoot.GetList;
using CookBook.Clean.UseCases.IngredientRoot.Update;
using CookBook.Clean.UseCases.Mappers;
using MediatR;
using Moq;

namespace CookBook.Clean.UnitTests.UseCases.Ingredients;

public class IngredientUnitTests
{
    [Fact]
    public async Task CreateIngredientHandler_InsertsEntityAndReturnsId()
    {
        // Arrange
        var repoMock = new Mock<IRepository<IngredientEntity>>();
        var mapperMock = new Mock<IIngredientMapper>();
        
        var expectedId = Guid.NewGuid();
        repoMock.Setup(r => r.InsertAsync(It.IsAny<IngredientEntity>()))
            .ReturnsAsync(expectedId);

        var handler = new CreateIngredientHandler(repoMock.Object, mapperMock.Object);
        var useCase = new CreateIngredientUseCase("Sugar", "Sweet", "http://img");

        // Act
        var result = await handler.Handle(useCase, CancellationToken.None);

        // Assert
        Assert.Equal(expectedId, result.Value);
        repoMock.Verify(r => r.InsertAsync(It.Is<IngredientEntity>(e => e.Name == "Sugar" && e.ImageUrl == "http://img")), Times.Once);
    }

    [Fact]
    public async Task GetIngredientHandler_ReturnsNotFound_WhenMissing()
    {
        var repoMock = new Mock<IRepository<IngredientEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((IngredientEntity?)null);
        var mapperMock = new Mock<IIngredientMapper>();

        var handler = new GetIngredientHandler(repoMock.Object, mapperMock.Object);
        var useCase = new GetIngredientUseCase(Guid.NewGuid());

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("Ingredient not found", result.Error);
    }

    [Fact]
    public async Task GetIngredientHandler_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        var entity = new IngredientEntity("Salt", null, "img") { Id = id };

        var repoMock = new Mock<IRepository<IngredientEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        var mapperMock = new Mock<IIngredientMapper>();

        var handler = new GetIngredientHandler(repoMock.Object, mapperMock.Object);
        var useCase = new GetIngredientUseCase(id);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal(id, result.Value!.Id);
        Assert.Equal("Salt", result.Value.Name);
    }

    [Fact]
    public async Task GetListIngredientHandler_ReturnsList()
    {
        var list = new List<IngredientEntity>
        {
            new IngredientEntity("A", null, null) { Id = Guid.NewGuid() },
            new IngredientEntity("B", null, null) { Id = Guid.NewGuid() }
        };

        var repoMock = new Mock<IRepository<IngredientEntity>>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
        var mapperMock = new Mock<IIngredientMapper>();

        var handler = new GetListIngredientHandler(repoMock.Object, mapperMock.Object);
        var useCase = new GetListIngredientUseCase();

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task UpdateIngredientHandler_ReturnsNotFound_WhenMissing()
    {
        var repoMock = new Mock<IRepository<IngredientEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((IngredientEntity?)null);

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdateIngredientHandler(repoMock.Object, publisherMock.Object);
        var useCase = new UpdateIngredientUseCase(Guid.NewGuid(), "New", null, null);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("Ingredient not found", result.Error);
    }

    [Fact]
    public async Task UpdateIngredientHandler_UpdatesAndReturnsOk()
    {
        var id = Guid.NewGuid();
        var entity = new IngredientEntity("Old", "d", null) { Id = id };

        var repoMock = new Mock<IRepository<IngredientEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        repoMock.Setup(r => r.UpdateAsync(It.IsAny<IngredientEntity>())).ReturnsAsync(id);

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdateIngredientHandler(repoMock.Object, publisherMock.Object);
        var useCase = new UpdateIngredientUseCase(id, "New", "NewDesc", "NewImg");

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.Success);
        repoMock.Verify(r => r.UpdateAsync(It.Is<IngredientEntity>(e => e.Name == "New" && e.Description == "NewDesc" && e.ImageUrl == "NewImg")), Times.Once);
        publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteIngredientHandler_DeletesAndReturnsOk()
    {
        var id = Guid.NewGuid();
        var repoMock = new Mock<IRepository<IngredientEntity>>();
        var repoMockRecipe = new Mock<IRecipeRepository>();
        repoMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteIngredientHandler(repoMock.Object, repoMockRecipe.Object, publisherMock.Object);
        var useCase = new DeleteIngredientUseCase(id);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.Success);
        repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
        publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
