using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record RemoveReviewFromRecipeCommand(RecipeId RecipeId, RecipeReviewId ReviewId) : ICommand;

internal sealed class RemoveReviewFromRecipeCommandHandler(IRepository<Recipe, RecipeId> recipeRepository)
    : ICommandHandler<RemoveReviewFromRecipeCommand>
{
    public async Task<Result> Handle(RemoveReviewFromRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.Failure(RecipeErrors.RecipeNotFoundError(request.RecipeId));
        }

        var result = recipe.RemoveReview(request.ReviewId);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }
}
