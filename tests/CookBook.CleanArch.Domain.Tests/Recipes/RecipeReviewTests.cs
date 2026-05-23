using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Tests.Recipes;

public class RecipeReviewTests
{
    [Fact]
    public void AddReview_WhenReviewIsValid_ShouldReturnSuccessAndAppendReview()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();

        // Act
        var result = recipe.AddReview(5, "Excellent coffee.");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(recipe.Reviews);

        var review = recipe.Reviews.Single();
        Assert.Equal(result.Value, review.Id);
        Assert.Equal(recipe.Id, review.RecipeId);
        Assert.Equal(5, review.Mark);
        Assert.Equal("Excellent coffee.", review.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void AddReview_WhenMarkIsOutsideAllowedRange_ShouldReturnFailureAndNotAppendReview(int invalidMark)
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();

        // Act
        var result = recipe.AddReview(invalidMark, "Nice.");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeReviewMarkOutOfRangeError(invalidMark), result.Error);
        Assert.Empty(recipe.Reviews);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void AddReview_WhenDescriptionIsBlank_ShouldReturnFailureAndNotAppendReview(string invalidDescription)
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();

        // Act
        var result = recipe.AddReview(4, invalidDescription);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeReviewDescriptionRequiredError(), result.Error);
        Assert.Empty(recipe.Reviews);
    }

    [Fact]
    public void AddReview_WhenDescriptionIsTooLong_ShouldReturnFailureAndNotAppendReview()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        var description = new string('a', Recipe.MaxReviewDescriptionLength + 1);

        // Act
        var result = recipe.AddReview(4, description);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeReviewDescriptionTooLongError(), result.Error);
        Assert.Empty(recipe.Reviews);
    }

    [Fact]
    public void AverageMark_WhenRecipeHasNoReviews_ShouldBeNull()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();

        // Assert
        Assert.Null(recipe.AverageMark);
    }

    [Fact]
    public void AverageMark_WhenRecipeHasReviews_ShouldReturnAverageOfMarks()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();

        // Act
        recipe.AddReview(5, "Excellent.");
        recipe.AddReview(3, "Fine.");
        recipe.AddReview(4, "Good.");

        // Assert
        Assert.Equal(4m, recipe.AverageMark);
    }

    [Fact]
    public void RemoveReview_WhenReviewExists_ShouldReturnSuccessAndRemoveOnlyTargetReview()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        var firstReviewId = recipe.AddReview(5, "Excellent.").Value;
        var secondReviewId = recipe.AddReview(2, "Too bitter.").Value;

        // Act
        var result = recipe.RemoveReview(firstReviewId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(recipe.Reviews);
        Assert.DoesNotContain(recipe.Reviews, review => review.Id == firstReviewId);
        Assert.Contains(recipe.Reviews, review => review.Id == secondReviewId);
        Assert.Equal(2m, recipe.AverageMark);
    }

    [Fact]
    public void RemoveReview_WhenReviewDoesNotExist_ShouldReturnFailureAndNotChangeReviews()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        var reviewId = recipe.AddReview(5, "Excellent.").Value;
        var missingReviewId = new RecipeReviewId(Guid.NewGuid());

        // Act
        var result = recipe.RemoveReview(missingReviewId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeReviewNotFoundError(missingReviewId, recipe.Id), result.Error);
        Assert.Single(recipe.Reviews);
        Assert.Contains(recipe.Reviews, review => review.Id == reviewId);
    }
}
