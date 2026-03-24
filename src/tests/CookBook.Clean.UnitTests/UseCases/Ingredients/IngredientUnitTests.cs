using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Application.Specifications;
using CookBook.Clean.Core.RecipeRoot;
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
        var repoMock = new Mock<IRepository<Ingredient>>();
        var mapper = new ManualIngredientMapper();
        
        var expectedId = Guid.NewGuid();
        repoMock.Setup(r => r.InsertAsync(It.IsAny<Ingredient>()))
            .ReturnsAsync(expectedId);

        var handler = new CreateIngredientCommandHandler(repoMock.Object, mapper);
        var useCase = new CreateIngredientCommand("Sugar", "Sweet", "http://a.png");

        // Act
        var result = await handler.Handle(useCase, CancellationToken.None);

        // Assert
        Assert.Equal(expectedId, result.Value);
        repoMock.Verify(r => r.InsertAsync(It.Is<Ingredient>(e => e.Name == "Sugar" && e.ImageUrl != null && e.ImageUrl.Value == "http://a.png")), Times.Once);
    }

    [Fact]
    public async Task GetIngredientHandler_ReturnsNotFound_WhenMissing()
    {
        var repoMock = new Mock<IRepository<Ingredient>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);
        var mapperMock = new Mock<IIngredientMapper>();

        var handler = new GetIngredientDetailQueryHandler(repoMock.Object, mapperMock.Object);
        var useCase = new GetIngredientDetailQuery(Guid.NewGuid());

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("Ingredient not found", result.Error);
    }

    [Fact]
    public async Task GetIngredientHandler_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        var entity = Ingredient.Create("Salt", null, ImageUrl.CreateObject("http://image.png").Value).Value;
        entity.Id = id;

        var repoMock = new Mock<IRepository<Ingredient>>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        var mapper = new ManualIngredientMapper();

        var handler = new GetIngredientDetailQueryHandler(repoMock.Object, mapper);
        var useCase = new GetIngredientDetailQuery(id);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value!.Id);
        Assert.Equal("Salt", result.Value.Name);
    }

    [Fact]
    public async Task GetListIngredientHandler_ReturnsList()
    {
        var entityA = Ingredient.Create("A", null, null).Value;
        entityA.Id = Guid.NewGuid();
        var entityB = Ingredient.Create("B", null, null).Value;
        entityB.Id = Guid.NewGuid();

        var list = new List<Ingredient>
        {
            entityA,
            entityB
        };

        var repoMock = new Mock<IRepository<Ingredient>>();
        repoMock.Setup(r => r.GetListBySpecificationAsync(It.IsAny<ISpecification<Ingredient, Ingredient>>()))
            .ReturnsAsync(list);
        var mapper = new ManualIngredientMapper();

        var handler = new GetIngredientListQueryHandler(repoMock.Object, mapper);
        var useCase = new GetIngredientListQuery(new IngredientFilter());

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task UpdateIngredientHandler_ReturnsNotFound_WhenMissing()
    {
        var repoMock = new Mock<IRepository<Ingredient>>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdateIngredientCommandHandler(repoMock.Object, publisherMock.Object);
        var useCase = new UpdateIngredientCommand(Guid.NewGuid(), "New", null, null);

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("Ingredient not found", result.Error);
    }

    [Fact]
    public async Task UpdateIngredientHandler_UpdatesAndReturnsOk()
    {
        var id = Guid.NewGuid();
        var entity = Ingredient.Create("Old", "d", null).Value;
        entity.Id = id;

        var repoMock = new Mock<IRepository<Ingredient>>();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        repoMock.Setup(r => r.UpdateAsync(It.IsAny<Ingredient>())).ReturnsAsync(id);

        var publisherMock = new Mock<IPublisher>();

        var handler = new UpdateIngredientCommandHandler(repoMock.Object, publisherMock.Object);
        var useCase = new UpdateIngredientCommand(id, "New", "NewDesc", "http://a.png");

        var result = await handler.Handle(useCase, CancellationToken.None);

        Assert.True(result.IsSuccess);
        repoMock.Verify(r => r.UpdateAsync(It.Is<Ingredient>(e => e.Name == "New" && e.Description == "NewDesc" && e.ImageUrl != null && e.ImageUrl.Value == "http://a.png")), Times.Once);
        publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
