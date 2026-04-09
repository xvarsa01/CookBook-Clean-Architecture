using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.Errors;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Commands;

public record RemoveIngredientFromRecipeCommand(RecipeId RecipeId, RecipeIngredientId EntryId) : ICommand;

internal sealed class RemoveIngredientFromRecipeCommandHandler(IRepository<Recipe, RecipeId > recipeRepository)
    : ICommandHandler<RemoveIngredientFromRecipeCommand>
{
    public async Task<Result> Handle(RemoveIngredientFromRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound(RecipeErrors.RecipeNotFoundError(request.RecipeId));
        }
        
        var result = recipe.RemoveIngredientByEntryId(request.EntryId);
        if (result.IsFailure)
            return Result.Invalid(result.Error);
            
        return Result.Ok();
    }
}
