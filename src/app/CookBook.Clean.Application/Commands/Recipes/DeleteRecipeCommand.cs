using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Commands.Recipes;

public record DeleteRecipeCommand(Guid Id) : ICommand;

internal sealed class DeleteRecipeCommandHandler(IRepository<Recipe> repository) : ICommandHandler<DeleteRecipeCommand>
{
    public async Task<Result> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return Result.Ok();
    }
}
