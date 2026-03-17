using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Update;

public class UpdateRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<UpdateRecipeUseCase, UseCaseResult<Guid>>
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
            existing.UpdateName(request.NewName);
        }
        if (request.NewDescription is not null)
        {
            existing.UpdateDescription(request.NewDescription);
        }
        
        existing.UpdateRest(request.NewImageUrl, request.NewDuration , request.NewType);

        var id = await repository.UpdateAsync(existing);
        if (id is null)
        {
            return UseCaseResult<Guid>.Invalid("Update failed");
        }
        
        return UseCaseResult<Guid>.Ok(id.Value);
    }
}
