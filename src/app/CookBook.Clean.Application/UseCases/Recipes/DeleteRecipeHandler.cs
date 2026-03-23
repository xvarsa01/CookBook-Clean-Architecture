using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record DeleteRecipeUseCase(Guid Id) : ICommand;

internal class DeleteRecipeHandler(IRepository<RecipeEntity> repository) : ICommandHandler<DeleteRecipeUseCase>
{
    public async Task<Result> Handle(DeleteRecipeUseCase request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return Result.Ok();
    }
}
