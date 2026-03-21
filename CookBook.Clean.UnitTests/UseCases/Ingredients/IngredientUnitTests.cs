using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Application.Specifications;
using CookBook.Clean.Application.UseCases.Ingredients;
using CookBook.Clean.Core.RecipeRoot;
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
        var mapper = new ManualIngredientMapper();
        
        var expectedId = Guid.NewGuid();
        repoMock.Setup(r => r.InsertAsync(It.IsAny<IngredientEntity>()))
            .ReturnsAsync(expectedId);

        var handler = new CreateIngredientHandler(repoMock.Object, mapper);
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

        var handler = new GetIngredientDetailHandler(repoMock.Object, mapperMock.Object);
        var useCase = new GetIngredientDetailQuery(Guid.NewGuid());

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
        var mapper = new ManualIngredientMapper();

        var handler = new GetIngredientDetailHandler(repoMock.Object, mapper);
        var useCase = new GetIngredientDetailQuery(id);

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
        repoMock.Setup(r => r.GetListBySpecificationAsync(It.IsAny<ISpecification<IngredientEntity, IngredientEntity>>()))
            .ReturnsAsync(list);
        var mapper = new ManualIngredientMapper();

        var handler = new GetIngredientListHandler(repoMock.Object, mapper);
        var useCase = new GetIngredientListQuery(new IngredientFilter());

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
        var entity = new IngredientEntity("Old", "d", null) { Id = id };
        
        var repoMock = new Mock<IRepository<IngredientEntity>>();
        var repoMockRecipe = new Mock<IRecipeRepository>();
        
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);
        repoMockRecipe.Setup(r => r.GetAllContainingIngredientAsync(id)).Returns(Task.FromResult(new List<RecipeEntity>()));

        var publisherMock = new Mock<IPublisher>();

        var handler = new DeleteIngredientHandler(repoMock.Object, repoMockRecipe.Object, publisherMock.Object);
        var useCase = new DeleteIngredientUseCase(id);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.Success);
        repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
        publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
