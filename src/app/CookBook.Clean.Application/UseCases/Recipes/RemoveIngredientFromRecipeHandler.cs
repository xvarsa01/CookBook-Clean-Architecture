using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record RemoveIngredientFromRecipeUseCase(Guid RecipeId, Guid EntryId) : ICommand;

internal class RemoveIngredientFromRecipeHandler(IRepository<RecipeEntity> recipeRepository)
    : ICommandHandler<RemoveIngredientFromRecipeUseCase>
{
    public async Task<Result> Handle(RemoveIngredientFromRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetByIdAsync(request.RecipeId);
        if (recipe is null)
        {
            return Result.NotFound("Recipe not found");
        }
        
        var result = recipe.RemoveIngredientByEntryId(request.EntryId);
        if (result.IsFailure)
            return Result.Invalid(result.Error ?? string.Empty);
            
        await recipeRepository.UpdateAsync(recipe);
        return Result.Ok();
    }
}
