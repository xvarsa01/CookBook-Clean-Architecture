using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.Errors;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Commands.Recipes;

public record UpdateIngredientInRecipeCommand(RecipeId RecipeId, RecipeUpdateIngredientRequest Request) : ICommand;

internal sealed class UpdateRecipeIngredientCommandHandler(IRepository<Recipe, RecipeId> recipeRepository) : ICommandHandler<UpdateIngredientInRecipeCommand>
{

    public async Task<Result> Handle(UpdateIngredientInRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound(RecipeErrors.RecipeNotFoundError(request.RecipeId));
        }
        
        var result = recipe.UpdateIngredientEntry(request.Request.EntryId, request.Request.NewAmount, request.Request.NewUnit);
        if (result.IsFailure)
            return Result.Invalid(result.Error);
        
        await recipeRepository.UpdateAsync(recipe);
        
        return Result.Ok();
    }
}
