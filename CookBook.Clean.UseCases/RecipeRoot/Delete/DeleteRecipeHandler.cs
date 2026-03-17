using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Delete;


public class DeleteRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<DeleteRecipeUseCase, UseCaseResult>
{
    public async Task<UseCaseResult> Handle(DeleteRecipeUseCase request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return UseCaseResult.Ok();
    }
}
