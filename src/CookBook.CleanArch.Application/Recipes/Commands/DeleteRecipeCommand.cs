using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using MediatR;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record DeleteRecipeCommand(RecipeId Id) : ICommand;

internal sealed class DeleteRecipeCommandHandler(IRepository<Recipe, RecipeId> repository, IPublisher publisher) : ICommandHandler<DeleteRecipeCommand>
{
    public async Task<Result> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        
        var recipeDeletedEvent = new RecipeDeletedEvent(request.Id.Value);
        await publisher.Publish(recipeDeletedEvent, cancellationToken);
        
        return Result.Ok();
    }
}
