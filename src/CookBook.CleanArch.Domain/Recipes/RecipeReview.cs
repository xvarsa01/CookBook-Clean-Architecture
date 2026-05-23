using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes;

public record RecipeReview : EntityBase<RecipeReviewId>
{
    public RecipeId RecipeId { get; init; }
    public int Mark { get; private set; }
    public string Description { get; private set; }

    private RecipeReview(RecipeReviewId id, RecipeId recipeId, int mark, string description) : base(id)
    {
        RecipeId = recipeId;
        Mark = mark;
        Description = description;
    }

    internal static Result<RecipeReview> Create(RecipeId recipeId, int mark, string description)
    {
        if (mark is < Recipe.MinReviewMark or > Recipe.MaxReviewMark)
            return Result.Failure<RecipeReview>(RecipeReviewErrors.RecipeReviewMarkOutOfRangeError(mark));

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<RecipeReview>(RecipeReviewErrors.RecipeReviewDescriptionRequiredError());

        if (description.Length > Recipe.MaxReviewDescriptionLength)
            return Result.Failure<RecipeReview>(RecipeReviewErrors.RecipeReviewDescriptionTooLongError());

        var id = new RecipeReviewId(Guid.NewGuid());
        return Result.Success(new RecipeReview(id, recipeId, mark, description));
    }
}
