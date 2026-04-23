using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.UnitTests.Ingredients.Commands;

public class UpdateIngredientCommandTests : UnitTestsBase
{
    [Fact]
    public async Task UpdateIngredientCommand_WhenAllFieldsProvided_UpdatesAllProperties()
    {
        // Arrange
        var ingredient = IngredientTestSeeds.IngredientForTestOfUpdate;
        var request = new IngredientUpdateRequest(
            Id: ingredient.Id,
            Name: "updated name",
            Description: "updated description",
            ImageUrl: ImageUrl.CreateObject("https://updated.com/img.jpg").Value);
        var command = new UpdateIngredientCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new IngredientId(ingredient.Id), result.Value);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var updated = await dbxAssert.Ingredients
            .SingleAsync(i => i.Id == ingredient.Id);

        Assert.Equal(request.Name, updated.Name);
        Assert.Equal(request.Description, updated.Description);
        Assert.Equal(request.ImageUrl!.Value, updated.ImageUrl!.Value);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenOnlyNameProvided_UpdatesOnlyName()
    {
        // Arrange
        var ingredient = IngredientTestSeeds.IngredientForTestOfUpdate;
        var request = new IngredientUpdateRequest(
            Id: ingredient.Id,
            Name: "new name only",
            Description: null,
            ImageUrl: null);
        var command = new UpdateIngredientCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var updated = await dbxAssert.Ingredients
            .SingleAsync(i => i.Id == ingredient.Id);

        Assert.Equal(request.Name, updated.Name);
        Assert.Equal(ingredient.Description, updated.Description);
        Assert.Equal(ingredient.ImageUrl, updated.ImageUrl);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenNoFieldsProvided_DoesNotChangeAnything()
    {
        // Arrange
        var ingredient = IngredientTestSeeds.IngredientForTestOfUpdate;
        var request = new IngredientUpdateRequest(
            Id: ingredient.Id,
            Name: null,
            Description: null,
            ImageUrl: null);
        var command = new UpdateIngredientCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var unchanged = await dbxAssert.Ingredients
            .SingleAsync(i => i.Id == ingredient.Id);

        Assert.Equal(ingredient.Name, unchanged.Name);
        Assert.Equal(ingredient.Description, unchanged.Description);
        Assert.Equal(ingredient.ImageUrl, unchanged.ImageUrl);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenIngredientDoesNotExist_ReturnsNotFoundFailure()
    {
        // Arrange
        var request = new IngredientUpdateRequest(
            Id: new IngredientId(Guid.NewGuid()),
            Name: "does not matter",
            Description: null,
            ImageUrl: null);
        var command = new UpdateIngredientCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IngredientErrors.IngredientNotFoundError(new IngredientId(request.Id)), result.Error);
    }

    [Fact]
    public async Task UpdateIngredientCommand_WhenInvalidNameProvided_ReturnsFailure_AndDoesNotPersistChanges()
    {
        // Arrange
        var ingredient = IngredientTestSeeds.IngredientForTestOfUpdate;
        var request = new IngredientUpdateRequest(
            Id: ingredient.Id,
            Name: string.Empty, // invalid
            Description: "new description",
            ImageUrl: null);
        var command = new UpdateIngredientCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var unchanged = await dbxAssert.Ingredients
            .SingleAsync(i => i.Id == ingredient.Id);

        Assert.Equal(ingredient.Name, unchanged.Name);
        Assert.Equal(ingredient.Description, unchanged.Description);
        Assert.Equal(ingredient.ImageUrl, unchanged.ImageUrl);
    }
}
