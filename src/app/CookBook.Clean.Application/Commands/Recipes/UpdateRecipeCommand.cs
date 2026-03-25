using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.Errors;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Commands.Recipes;

public record UpdateRecipeCommand(RecipeUpdateRequest Request) : ICommand<Guid>;

internal sealed class UpdateRecipeCommandHandler(IRepository<Recipe, RecipeId> repository) : ICommandHandler<UpdateRecipeCommand,Guid>
{
    public async Task<Result<Guid>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Request.Id);
        if (existing is null)
        {
            return Result.NotFound<Guid>(RecipeErrors.RecipeNotFoundError(new RecipeId(request.Request.Id)));
        }

        if (request.Request.Name is not null)
        {
            var result = existing.UpdateName(request.Request.Name);
            if (result.IsFailure)
                return Result.Invalid<Guid>(result.Error);
        }
        
        if (request.Request.Description is not null)
        {
            var result = existing.UpdateDescription(request.Request.Description);
            if (result.IsFailure)
                return Result.Invalid<Guid>(result.Error);
        }
        
        var restResult = existing.UpdateRest(request.Request.ImageUrl, request.Request.Duration, request.Request.Type);
        if (restResult.IsFailure)
            return Result.Invalid<Guid>(restResult.Error);
        
        var id = await repository.UpdateAsync(existing);
        if (id is null)
        {
            return Result.Invalid<Guid>(RecipeErrors.RecipeUpdateFailedError(new RecipeId(request.Request.Id)));
        }
        
        return Result.Ok(id.Value);
    }
}
