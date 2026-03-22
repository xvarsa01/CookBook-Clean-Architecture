using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

internal class UpdateRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<UpdateRecipeUseCase, UseCaseResult<Guid>>
{
    public async Task<UseCaseResult<Guid>> Handle(UpdateRecipeUseCase request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return UseCaseResult<Guid>.NotFound("Recipe not found");
        }

        if (request.NewName is not null)
        {
            existing.UpdateName(new RecipeName(request.NewName));
        }
        
        if (request.NewDescription is not null)
        {
            existing.UpdateDescription(request.NewDescription);
        }

        var duration = request.NewDuration is not null 
            ? new RecipeDuration(request.NewDuration.Value) 
            : null;
        
        var url = request.NewImageUrl is not null
            ? new ImageUrl(request.NewImageUrl)
            : null;
        
        existing.UpdateRest(url, duration , request.NewType);

        var id = await repository.UpdateAsync(existing);
        if (id is null)
        {
            return UseCaseResult<Guid>.Invalid("Update failed");
        }
        
        return UseCaseResult<Guid>.Ok(id.Value);
    }
}
