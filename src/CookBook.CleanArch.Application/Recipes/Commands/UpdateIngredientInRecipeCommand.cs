using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record UpdateIngredientInRecipeCommand(RecipeId RecipeId, RecipeUpdateIngredientRequest Request) : ICommand;

internal sealed class UpdateRecipeIngredientCommandHandler(IRepository<Recipe, RecipeId> recipeRepository) : ICommandHandler<UpdateIngredientInRecipeCommand>
{

    public async Task<Result> Handle(UpdateIngredientInRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.Failure(RecipeErrors.RecipeNotFoundError(request.RecipeId));
        }
        
        var result = recipe.UpdateIngredientEntry(request.Request.EntryId, request.Request.NewAmount, request.Request.NewUnit);
        if (result.IsFailure)
            return Result.Failure(result.Error);
        
        return Result.Success();
    }
}
