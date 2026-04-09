using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.Errors;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Commands;

public record UpdateRecipeCommand(RecipeUpdateRequest Request) : ICommand<RecipeId>;

internal sealed class UpdateRecipeCommandHandler(IRepository<Recipe, RecipeId> repository) : ICommandHandler<UpdateRecipeCommand,RecipeId>
{
    public async Task<Result<RecipeId>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Request.Id);
        if (existing is null)
        {
            return Result.NotFound<RecipeId>(RecipeErrors.RecipeNotFoundError(request.Request.Id));
        }

        if (request.Request.Name is not null)
        {
            var result = existing.UpdateName(request.Request.Name);
            if (result.IsFailure)
                return Result.Invalid<RecipeId>(result.Error);
        }
        
        if (request.Request.Description is not null)
        {
            var result = existing.UpdateDescription(request.Request.Description);
            if (result.IsFailure)
                return Result.Invalid<RecipeId>(result.Error);
        }
        
        var restResult = existing.UpdateRest(request.Request.ImageUrl, request.Request.Duration, request.Request.Type);
        if (restResult.IsFailure)
            return Result.Invalid<RecipeId>(restResult.Error);
        
        var id = await repository.UpdateAsync(existing);
        if (id is null)
        {
            return Result.Invalid<RecipeId>(RecipeErrors.RecipeUpdateFailedError(request.Request.Id));
        }
        
        return Result.Ok(id);
    }
}
