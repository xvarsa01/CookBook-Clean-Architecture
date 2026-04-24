using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Tests.Ingredients.Commands;

public class CreateIngredientCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task CreateIngredientCommand_WithAllProperties_PersistsIngredient()
    {
        // Arrange
        var request = new IngredientCreateRequest(
            Name: "new ingredient",
            Description: "new ingredient description",
            ImageUrl: ImageUrl.CreateObject("https://example.com/image.jpg").Value);
        var command = new CreateIngredientCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var createdIngredient = await dbxAssert.Ingredients
            .SingleAsync(i => i.Name == request.Name, CancellationToken.None);

        Assert.Equal(request.Name, createdIngredient.Name);
        Assert.Equal(request.Description, createdIngredient.Description);
        Assert.NotNull(createdIngredient.ImageUrl);
        Assert.Equal(request.ImageUrl!.Value, createdIngredient.ImageUrl!.Value);
    }

    [Fact]
    public async Task CreateIngredientCommand_WithNullableOptionalFields_PersistsIngredient()
    {
        // Arrange
        var request = new IngredientCreateRequest(
            Name: "new ingredient",
            Description: null,
            ImageUrl: null);
        var command = new CreateIngredientCommand(request);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var createdIngredient = await dbxAssert.Ingredients
            .SingleAsync(i => i.Name == request.Name, CancellationToken.None);

        Assert.Equal(request.Name, createdIngredient.Name);
        Assert.Null(createdIngredient.Description);
        Assert.Null(createdIngredient.ImageUrl);
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
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
    }
}
