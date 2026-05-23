using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Tests.Recipes.Commands;

public class AddReviewToRecipeCommandTests : ApplicationTestsBase
{
    [Fact]
    public async Task AddReviewToRecipeCommand_WithValidData_AddsReview()
    {
        // Arrange
        var recipeId = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name).Id;
        var command = new AddReviewToRecipeCommand(
            recipeId,
            new RecipeAddReviewRequest(5, "Excellent coffee."));

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);

        await using var db = await DbContextFactory.CreateDbContextAsync();
        var recipe = await db.Recipes
            .Include(r => r.Reviews)
            .SingleAsync(r => r.Id == recipeId);

        var review = Assert.Single(recipe.Reviews);
        Assert.Equal(result.Value, review.Id);
        Assert.Equal(5, review.Mark);
        Assert.Equal("Excellent coffee.", review.Description);
    }

    [Fact]
    public async Task AddReviewToRecipeCommand_WithNonExistingRecipe_ReturnsFailure()
    {
        // Arrange
        var recipeId = new RecipeId(Guid.NewGuid());
        var command = new AddReviewToRecipeCommand(
            recipeId,
            new RecipeAddReviewRequest(5, "Excellent coffee."));

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeNotFoundError(recipeId), result.Error);
    }

    [Fact]
    public async Task AddReviewToRecipeCommand_WithInvalidMark_ReturnsFailure()
    {
        // Arrange
        var recipeId = GetSeededRecipeByName(RecipeTestSeeds.RecipeWithSingleIngredient().Name).Id;
        var command = new AddReviewToRecipeCommand(
            recipeId,
            new RecipeAddReviewRequest(6, "Excellent coffee."));

        // Act
        var result = await Mediator.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeReviewMarkOutOfRangeError(6), result.Error);
    }
}
