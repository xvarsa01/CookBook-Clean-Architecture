using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.Events;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using MediatR;

namespace CookBook.CleanArch.Application.Ingredients.Commands;

public record DeleteIngredientCommand(IngredientId Id) : ICommand;

internal sealed class DeleteIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository, IPublisher publisher, IMediator mediator)
    : ICommandHandler<DeleteIngredientCommand>
{
    public async Task<Result> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return Result.NotFound(IngredientErrors.IngredientNotFoundError(request.Id));
        }
        
        var recipesContainingIngredient = await mediator.Send(new GetRecipeListByContainingIngredientIdQuery(request.Id), cancellationToken);
        if (recipesContainingIngredient.IsFailure)
        {
            return Result.Invalid(recipesContainingIngredient.Error);
        }
        
        if (recipesContainingIngredient.Value.Count != 0)
        {
            return Result.Invalid(IngredientErrors.IngredientIsUsedAndCanNotBeDeletedError(recipesContainingIngredient.Value.Count));
        }
        
        await repository.DeleteAsync(request.Id);

        var ingredientDeletedEvent = new IngredientDeletedEvent(request.Id.Value);
        await publisher.Publish(ingredientDeletedEvent, cancellationToken);

        return Result.Ok();
    }
}
