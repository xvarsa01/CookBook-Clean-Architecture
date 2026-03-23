using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;


internal class DeleteRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<DeleteRecipeUseCase, Result>
{
    public async Task<Result> Handle(DeleteRecipeUseCase request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return Result.Ok();
    }
}
