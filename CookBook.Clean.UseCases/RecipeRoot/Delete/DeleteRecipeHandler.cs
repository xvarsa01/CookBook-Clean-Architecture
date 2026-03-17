using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Delete;

public record DeleteRecipeResult;

public class DeleteRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<DeleteRecipeUseCase, UseCaseResult<DeleteRecipeResult>>
{
    public async Task<UseCaseResult<DeleteRecipeResult>> Handle(DeleteRecipeUseCase request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return UseCaseResult<DeleteRecipeResult>.Ok(new DeleteRecipeResult());
    }
}
