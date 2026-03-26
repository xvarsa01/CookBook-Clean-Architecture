using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Commands.Recipes;

public record DeleteRecipeCommand(RecipeId Id) : ICommand;

internal sealed class DeleteRecipeCommandHandler(IRepository<Recipe, RecipeId> repository) : ICommandHandler<DeleteRecipeCommand>
{
    public async Task<Result> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return Result.Ok();
    }
}
