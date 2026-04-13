using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using MediatR;

namespace CookBook.CleanArch.Application.Ingredients.Commands;

public record DeleteIngredientCommand(IngredientId Id) : ICommand;

internal sealed class DeleteIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository,  IPublisher publisher, IMediator mediator)
    : ICommandHandler<DeleteIngredientCommand>
{
    public async Task<Result> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        return (await repository.GetByIdAsync(request.Id)
            .EnsureNotNullNotFound(IngredientErrors.IngredientNotFoundError(request.Id))
            .Bind(_ => mediator.Send(new GetRecipeListByContainingIngredientIdQuery(request.Id), cancellationToken))
            .Ensure(
                recipesContainingIngredient => recipesContainingIngredient.Count == 0,
                recipesContainingIngredient => IngredientErrors.IngredientIsUsedAndCanNotBeDeletedError(recipesContainingIngredient.Count))
            .Tap(() => repository.DeleteAsync(request.Id)))
            .ToResult();
    }
}
