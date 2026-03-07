using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Update;

public record UpdateRecipeResult;

public class UpdateRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<UpdateRecipeUseCase, UseCaseResult<UpdateRecipeResult>>
{
    public async Task<UseCaseResult<UpdateRecipeResult>> Handle(UpdateRecipeUseCase request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return UseCaseResult<UpdateRecipeResult>.NotFound("Recipe not found");
        }

        if (request.NewName is not null)
        {
            existing.UpdateName(request.NewName);
        }
        if (request.NewDescription is not null)
        {
            existing.UpdateDescription(request.NewDescription);
        }
        if (request.NewImageUrl is not null)
        {
            existing.UpdateImageUrl(request.NewImageUrl);
        }
        if (request.NewType != existing.Type)
        {
            existing.UpdateType(request.NewType);
        }

        await repository.UpdateAsync(existing);
        return UseCaseResult<UpdateRecipeResult>.Ok(new UpdateRecipeResult());
    }
}
