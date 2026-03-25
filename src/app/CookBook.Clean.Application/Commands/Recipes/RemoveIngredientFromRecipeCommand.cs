using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Errors;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Commands.Recipes;

public record RemoveIngredientFromRecipeCommand(Guid RecipeId, Guid EntryId) : ICommand;

internal sealed class RemoveIngredientFromRecipeCommandHandler(IRepository<Recipe, RecipeId > recipeRepository)
    : ICommandHandler<RemoveIngredientFromRecipeCommand>
{
    public async Task<Result> Handle(RemoveIngredientFromRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound(RecipeErrors.RecipeNotFoundError(new RecipeId(request.RecipeId)));
        }
        
        var result = recipe.RemoveIngredientByEntryId(request.EntryId);
        if (result.IsFailure)
            return Result.Invalid(result.Error);
            
        await recipeRepository.UpdateAsync(recipe);
        return Result.Ok();
    }
}
