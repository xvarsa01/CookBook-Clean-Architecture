using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Events;
using MediatR;

namespace CookBook.Clean.Application.Commands.Ingredients;

public record DeleteIngredientCommand(Guid Id) : ICommand;

internal sealed class DeleteIngredientCommandHandler(IRepository<IngredientEntity> repository, IRecipeRepository recipeRepository, IPublisher publisher)
    : ICommandHandler<DeleteIngredientCommand>
{
    public async Task<Result> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return Result.NotFound("Ingredient not found");
        }
        
        var recipesContainingIngredient = await recipeRepository.GetAllContainingIngredientAsync(request.Id);

        if (recipesContainingIngredient.Count != 0)
        {
            return Result.Invalid("Cannot delete ingredient that is used in recipes. Remove it from all recipes first.");
        }
        
        await repository.DeleteAsync(request.Id);

        var ingredientDeletedEvent = new IngredientDeletedEvent(request.Id);
        await publisher.Publish(ingredientDeletedEvent, cancellationToken);

        return Result.Ok();
    }
}
