using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using NSubstitute;

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class RemoveReviewFromRecipeCommandTests
{
    private readonly IRecipeRepository _recipeRepositoryMock;
    private readonly RemoveReviewFromRecipeCommandHandler _handler;

    public RemoveReviewFromRecipeCommandTests()
    {
        _recipeRepositoryMock = Substitute.For<IRecipeRepository>();
        _handler = new RemoveReviewFromRecipeCommandHandler(_recipeRepositoryMock);
    }

    [Fact]
    public async Task RemoveReviewFromRecipeCommand_WithExistingReview_RemovesReview()
    {
        // Arrange
        var recipe = RecipeTestData.CreateRecipe();
        var reviewId = recipe.AddReview(4, "Good.").Value;
        var command = new RemoveReviewFromRecipeCommand(recipe.Id, reviewId);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(recipe.Reviews);
    }

    [Fact]
    public async Task RemoveReviewFromRecipeCommand_WithNonExistingRecipe_ReturnsFailure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var reviewId = new RecipeReviewId(Guid.NewGuid());
        var command = new RemoveReviewFromRecipeCommand(recipeId, reviewId);

        _recipeRepositoryMock.GetByIdAsync(recipeId).Returns((Recipe?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task RemoveReviewFromRecipeCommand_WithNonExistingReview_ReturnsFailure()
    {
        // Arrange
        var recipe = RecipeTestData.CreateRecipe();
        var reviewId = new RecipeReviewId(Guid.NewGuid());
        var command = new RemoveReviewFromRecipeCommand(recipe.Id, reviewId);

        _recipeRepositoryMock.GetByIdAsync(recipe.Id).Returns(recipe);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeReviewErrors.RecipeReviewNotFoundError(reviewId, recipe.Id), result.Error);
        Assert.Empty(recipe.Reviews);
    }
}
