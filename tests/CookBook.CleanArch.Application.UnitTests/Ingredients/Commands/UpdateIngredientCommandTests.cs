using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.UnitTests.Ingredients.Commands;

public class UpdateIngredientCommandTests
{
    private readonly IIngredientRepository _ingredientRepositoryMock;
    private readonly UpdateIngredientCommandHandler _handler;

    public UpdateIngredientCommandTests()
    {
        _ingredientRepositoryMock = Substitute.For<IIngredientRepository>();
        _handler = new UpdateIngredientCommandHandler(_ingredientRepositoryMock);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenAllFieldsProvided_UpdatesAllProperties()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var request = new IngredientUpdateRequest(
            ingredient.Id,
            "updated name",
            "updated description",
            ImageUrl.CreateObject("https://updated.com/img.jpg").Value);
        var command = new UpdateIngredientCommand(request);

        _ingredientRepositoryMock.GetByIdAsync(ingredient.Id).Returns(ingredient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ingredient.Id, result.Value);
        Assert.Equal(request.Name, ingredient.Name);
        Assert.Equal(request.Description, ingredient.Description);
        Assert.Equal(request.ImageUrl, ingredient.ImageUrl);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenOnlyNameProvided_UpdatesOnlyName()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var originalDescription = ingredient.Description;
        var originalImageUrl = ingredient.ImageUrl;
        var request = new IngredientUpdateRequest(
            ingredient.Id,
            "new name only",
            null,
            null);
        var command = new UpdateIngredientCommand(request);

        _ingredientRepositoryMock.GetByIdAsync(ingredient.Id).Returns(ingredient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Name, ingredient.Name);
        Assert.Equal(originalDescription, ingredient.Description);
        Assert.Equal(originalImageUrl, ingredient.ImageUrl);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenNoFieldsProvided_DoesNotChangeAnything()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var originalName = ingredient.Name;
        var originalDescription = ingredient.Description;
        var originalImageUrl = ingredient.ImageUrl;
        var request = new IngredientUpdateRequest(
            ingredient.Id,
            null,
            null,
            null);
        var command = new UpdateIngredientCommand(request);

        _ingredientRepositoryMock.GetByIdAsync(ingredient.Id).Returns(ingredient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(originalName, ingredient.Name);
        Assert.Equal(originalDescription, ingredient.Description);
        Assert.Equal(originalImageUrl, ingredient.ImageUrl);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenIngredientDoesNotExist_ReturnsNotFoundFailure()
    {
        // Arrange
        var ingredientId = new IngredientId(Guid.NewGuid());
        var request = new IngredientUpdateRequest(
            ingredientId,
            "does not matter",
            null,
            null);
        var command = new UpdateIngredientCommand(request);

        _ingredientRepositoryMock.GetByIdAsync(ingredientId).Returns((Ingredient?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(ingredientId), result.Error);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenInvalidNameProvided_ReturnsFailure_AndDoesNotUpdateIngredient()
    {
        // Arrange
        var ingredient = IngredientTestData.CreateIngredient();
        var originalName = ingredient.Name;
        var originalDescription = ingredient.Description;
        var originalImageUrl = ingredient.ImageUrl;
        var request = new IngredientUpdateRequest(
            ingredient.Id,
            string.Empty,
            "new description",
            null);
        var command = new UpdateIngredientCommand(request);

        _ingredientRepositoryMock.GetByIdAsync(ingredient.Id).Returns(ingredient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNameEmptyError(), result.Error);
        Assert.Equal(originalName, ingredient.Name);
        Assert.Equal(originalDescription, ingredient.Description);
        Assert.Equal(originalImageUrl, ingredient.ImageUrl);
    }
}
