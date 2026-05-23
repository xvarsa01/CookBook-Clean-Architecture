using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class RemoveReviewFromRecipeCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task RemoveReviewFromRecipeCommand_WithExistingReview_RemovesReview()
    {
        // Arrange
        var recipeId = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name).Id;
        var addResult = await Mediator.Send(new AddReviewToRecipeCommand(
            recipeId,
            new RecipeAddReviewRequest(4, "Good.")));

        DbContext.ChangeTracker.Clear();
        var command = new RemoveReviewFromRecipeCommand(recipeId, addResult.Value);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var recipe = await db.Recipes
            .Include(r => r.Reviews)
            .SingleAsync(r => r.Id == recipeId);

        Assert.Empty(recipe.Reviews);
    }

    [Fact]
    public async Task RemoveReviewFromRecipeCommand_WithNonExistingRecipe_ReturnsFailure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var reviewId = new RecipeReviewId(Guid.NewGuid());
        var command = new RemoveReviewFromRecipeCommand(recipeId, reviewId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task RemoveReviewFromRecipeCommand_WithNonExistingReview_ReturnsFailure()
    {
        // Arrange
        var recipeId = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name).Id;
        var reviewId = new RecipeReviewId(Guid.NewGuid());
        var command = new RemoveReviewFromRecipeCommand(recipeId, reviewId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeReviewNotFoundError(reviewId, recipeId), result.Error);
    }
}
