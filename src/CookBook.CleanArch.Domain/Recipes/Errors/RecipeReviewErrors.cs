using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Recipes.Errors;

public static class RecipeReviewErrors
{
    public static Error RecipeReviewNotFoundError(RecipeReviewId reviewId, RecipeId recipeId) =>
        new("Recipes.RecipeReviewNotFound", $"Review {reviewId.Value} not found in recipe {recipeId.Value}.", [reviewId, recipeId]);

    public static Error RecipeReviewMarkOutOfRangeError(int mark) =>
        new("Recipes.RecipeReviewMarkOutOfRange", $"Review mark must be between {Recipe.MinReviewMark} and {Recipe.MaxReviewMark}.", mark);

    public static Error RecipeReviewDescriptionRequiredError() =>
        new("Recipes.RecipeReviewDescriptionRequired", "Review description is required.");

    public static Error RecipeReviewDescriptionTooLongError() =>
        new("Recipes.RecipeReviewDescriptionTooLong", $"Review description can not be longer than {Recipe.MaxReviewDescriptionLength} characters.");
}
