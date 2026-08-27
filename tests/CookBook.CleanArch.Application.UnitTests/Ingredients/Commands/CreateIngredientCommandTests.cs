using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.UnitTests.Ingredients.Commands;

public class CreateIngredientCommandTests
{
    private readonly IRepository<Ingredient, IngredientId> _ingredientRepositoryMock;
    private readonly CreateIngredientCommandHandler _handler;

    public CreateIngredientCommandTests()
    {
        _ingredientRepositoryMock = Substitute.For<IRepository<Ingredient, IngredientId>>();
        _ingredientRepositoryMock
            .Add(Arg.Any<Ingredient>())
            .Returns(call => call.Arg<Ingredient>().Id);
        
        _handler = new CreateIngredientCommandHandler(_ingredientRepositoryMock);
    }

    [Fact]
    public async Task CreateIngredientCommand_WithAllProperties_AddsIngredientToRepository()
    {
        // Arrange
        var request = new IngredientCreateRequest(
            Name: "new ingredient",
            Description: "new ingredient description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value);
        var command = new CreateIngredientCommand(request);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        var addedIngredient = Arg.Is<Ingredient>(ingredient =>
            ingredient.Id == result.Value &&
            ingredient.Name == request.Name &&
            ingredient.Description == request.Description &&
            ingredient.ImageUrl == request.ImageUrl);
        
        _ingredientRepositoryMock.Received(1).Add(addedIngredient);
    }

    [Fact]
    public async Task CreateIngredientCommand_WithNullableOptionalFields_AddsIngredientToRepository()
    {
        // Arrange
        var request = new IngredientCreateRequest(
            Name: "new ingredient",
            Description: null,
            ImageUrl: null);
        var command = new CreateIngredientCommand(request);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var addedIngredient = Arg.Is<Ingredient>(ingredient =>
            ingredient.Id == result.Value &&
            ingredient.Name == request.Name &&
            ingredient.Description == request.Description &&
            ingredient.ImageUrl == request.ImageUrl);
        
        _ingredientRepositoryMock.Received(1).Add(addedIngredient);
    }
    
    [Fact]
    public async Task CreateIngredientCommand_WithEmptyName_Returns_Failure()
    {
        // Arrange
        var request = new IngredientCreateRequest(
            Name: string.Empty,
            Description: null,
            ImageUrl: null);
        var command = new CreateIngredientCommand(request);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        
        _ingredientRepositoryMock.DidNotReceive().Add(Arg.Any<Ingredient>());
    }
}
