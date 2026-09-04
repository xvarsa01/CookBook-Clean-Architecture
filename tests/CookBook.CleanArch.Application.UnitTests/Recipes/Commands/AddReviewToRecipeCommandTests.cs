using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.UnitTests.Recipes.Commands;

public class AddReviewToRecipeCommandTests
{
    private readonly IRecipeRepository _recipeRepositoryMock;
    private readonly AddReviewToRecipeCommandHandler _handler;

    public AddReviewToRecipeCommandTests()
    {
        _recipeRepositoryMock = Substitute.For<IRecipeRepository>();
        _handler = new AddReviewToRecipeCommandHandler(_recipeRepositoryMock);
    }

    [Fact]
    public async Task AddReviewToRecipeCommand_WithValidData_AddsReview()
    {
        // Arrange
        var recipe = RecipeTestData.CreateRecipe();
        var request = new AddRecipeReviewRequest(5, "Excellent coffee.");
        var command = new AddReviewToRecipeCommand(recipe.Id, request);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        var review = Assert.Single(recipe.Reviews);
        Assert.Equal(result.Value, review.Id);
        Assert.Equal(request.Rating, review.Mark);
        Assert.Equal(request.Comment, review.Description);
    }

    [Fact]
    public async Task AddReviewToRecipeCommand_WithNonExistingRecipe_ReturnsFailure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var command = new AddReviewToRecipeCommand(
            recipeId,
            new AddRecipeReviewRequest(5, "Excellent coffee."));

        _recipeRepositoryMock.GetByIdAsync(recipeId).Returns((Recipe?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task AddReviewToRecipeCommand_WithInvalidMark_ReturnsFailure()
    {
        // Arrange
        var recipe = RecipeTestData.CreateRecipe();
        var invalidMark = 6;
        var command = new AddReviewToRecipeCommand(
            recipe.Id,
            new AddRecipeReviewRequest(invalidMark, "Excellent coffee."));

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeReviewErrors.RecipeReviewMarkOutOfRangeError(invalidMark), result.Error);
        Assert.Empty(recipe.Reviews);
    }
}
