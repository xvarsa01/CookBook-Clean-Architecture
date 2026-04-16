using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record RemoveIngredientsFromRecipeByIngredientIdCommand(RecipeId RecipeId, IngredientId IngredientId) : ICommand;

internal sealed class RemoveIngredientFromRecipeByIngredientIdCommandHandler(IRepository<Recipe, RecipeId > recipeRepository)
    : ICommandHandler<RemoveIngredientsFromRecipeByIngredientIdCommand>
{
    public async Task<Result> Handle(RemoveIngredientsFromRecipeByIngredientIdCommand request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.Failure(RecipeErrors.RecipeNotFoundError(request.RecipeId));
        }
        
        var result = recipe.RemoveIngredientsByIngredientId(request.IngredientId);
        if (result.IsFailure)
            return Result.Failure(result.Error);
            
        return Result.Success();
    }
}
