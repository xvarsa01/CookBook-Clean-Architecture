using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.UnitTests.Recipes.Commands;

public class DeleteRecipeCommandTests
{
    private readonly IRecipeRepository _recipeRepositoryMock;
    private readonly DeleteRecipeCommandHandler _handler;

    public DeleteRecipeCommandTests()
    {
        _recipeRepositoryMock = Substitute.For<IRecipeRepository>();
        _handler = new DeleteRecipeCommandHandler(_recipeRepositoryMock);
    }

    [Fact]
    public async Task DeleteRecipeCommand_WithExistingRecipe_DeletesRecipe()
    {
        // Arrange
        var recipe = RecipeTestData.CreateRecipe();
        var command = new DeleteRecipeCommand(recipe.Id);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _recipeRepositoryMock.Received(1).Delete(recipe);
    }

    [Fact]
    public async Task DeleteRecipeCommand_WithNonExistingRecipe_ReturnsFailure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var command = new DeleteRecipeCommand(recipeId);

        _recipeRepositoryMock.GetByIdAsync(recipeId).Returns((Recipe?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
        _recipeRepositoryMock.DidNotReceive().Delete(Arg.Any<Recipe>());
    }
}
