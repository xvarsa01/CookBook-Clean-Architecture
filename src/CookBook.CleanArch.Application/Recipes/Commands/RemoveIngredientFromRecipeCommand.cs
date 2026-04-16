using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record RemoveIngredientFromRecipeCommand(RecipeId RecipeId, RecipeIngredientId EntryId) : ICommand;

internal sealed class RemoveIngredientFromRecipeCommandHandler(IRepository<Recipe, RecipeId > recipeRepository)
    : ICommandHandler<RemoveIngredientFromRecipeCommand>
{
    public async Task<Result> Handle(RemoveIngredientFromRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.Failure(RecipeErrors.RecipeNotFoundError(request.RecipeId));
        }
        
        var result = recipe.RemoveIngredientByEntryId(request.EntryId);
        if (result.IsFailure)
            return Result.Failure(result.Error);
            
        return Result.Success();
    }
}
