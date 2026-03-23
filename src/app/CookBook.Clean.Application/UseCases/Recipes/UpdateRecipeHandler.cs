using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record UpdateRecipeUseCase(Guid Id, string? NewName, string? NewDescription, string? NewImageUrl, TimeSpan? NewDuration, RecipeType? NewType) : IRequest<Result<Guid>>;

internal class UpdateRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<UpdateRecipeUseCase, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateRecipeUseCase request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return Result.NotFound<Guid>("Recipe not found");
        }

        if (request.NewName is not null)
        {
            var nameObjectResult = RecipeName.CreateObject(request.NewName);
            if (!nameObjectResult.IsSuccess)            {
                return Result.Invalid<Guid>(nameObjectResult.Error ?? string.Empty);
            }
            existing.UpdateName(nameObjectResult.Value);
        }
        
        if (request.NewDescription is not null)
        {
            existing.UpdateDescription(request.NewDescription);
        }

        var durationObjectResult = request.NewDuration is not null 
            ? RecipeDuration.CreateObject(request.NewDuration.Value) 
            : null;
        if (durationObjectResult != null && durationObjectResult.IsFailure)
        {
            return Result.Invalid<Guid>(durationObjectResult.Error ?? string.Empty);
        }
        
        var urlObjectResult = request.NewImageUrl is not null
            ? ImageUrl.CreateObject(request.NewImageUrl)
            : null;
        if (urlObjectResult != null && urlObjectResult.IsFailure)
        {
            return Result.Invalid<Guid>(urlObjectResult.Error ?? string.Empty);
        }
        
        existing.UpdateRest(urlObjectResult?.Value, durationObjectResult?.Value, request.NewType);

        var id = await repository.UpdateAsync(existing);
        if (id is null)
        {
            return Result.Invalid<Guid>("Update failed");
        }
        
        return Result.Ok(id.Value);
    }
}
