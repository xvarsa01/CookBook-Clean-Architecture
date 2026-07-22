using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record AddReviewToRecipeCommand(RecipeId RecipeId, AddRecipeReviewRequest Request) : ICommand<RecipeReviewId>;

internal sealed class AddReviewToRecipeCommandHandler(IRepository<Recipe, RecipeId> recipeRepository)
    : ICommandHandler<AddReviewToRecipeCommand, RecipeReviewId>
{
    public async Task<Result<RecipeReviewId>> Handle(AddReviewToRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.Failure<RecipeReviewId>(RecipeErrors.RecipeNotFoundError(request.RecipeId));
        }

        var result = recipe.AddReview(request.Request.Rating, request.Request.Comment);
        if (result.IsFailure)
        {
            return Result.Failure<RecipeReviewId>(result.Error);
        }

        return Result.Success(result.Value);
    }
}
