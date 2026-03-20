using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.Delete;


public class DeleteRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<DeleteRecipeUseCase, UseCaseResult>
{
    public async Task<UseCaseResult> Handle(DeleteRecipeUseCase request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return UseCaseResult.Ok();
    }
}
