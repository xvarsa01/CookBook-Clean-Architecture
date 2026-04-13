using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

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
        
        return Result.Ok(existing.Id);
    }
}
