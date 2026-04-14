using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record DeleteRecipeCommand(RecipeId Id) : ICommand;

internal sealed class DeleteRecipeCommandHandler(IRepository<Recipe, RecipeId> repository) : ICommandHandler<DeleteRecipeCommand>
{
    public async Task<Result> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await repository.GetByIdAsync(request.Id);
        if (recipe is null)
        {
            return Result.NotFound(RecipeErrors.RecipeNotFoundError(request.Id));
        }
        
        var deleteResult = recipe.Delete();
        if (deleteResult.IsFailure)
        {
            return deleteResult;
        }
           
        repository.Delete(recipe);

        return Result.Ok();
    }
}
