using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.Errors;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Commands.Recipes;

public record UpdateIngredientInRecipeCommand(Guid RecipeId, RecipeUpdateIngredientRequest Model) : ICommand;

internal sealed class UpdateIngredientInRecipeCommandHandler(IRepository<Recipe, RecipeId> recipeRepository) : ICommandHandler<UpdateIngredientInRecipeCommand>
{

    public async Task<Result> Handle(UpdateIngredientInRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound(RecipeErrors.RecipeNotFoundError(new RecipeId(request.RecipeId)));
        }
        
        var result = recipe.UpdateIngredientEntry(request.Model.EntryId, request.Model.NewAmount, request.Model.NewUnit);
        if (result.IsFailure)
            return Result.Invalid(result.Error);
        
        await recipeRepository.UpdateAsync(recipe);
        
        return Result.Ok();
    }
}
