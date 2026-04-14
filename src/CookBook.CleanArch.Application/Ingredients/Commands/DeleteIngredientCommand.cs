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
            .Bind(ingredient => mediator.Send(new GetRecipeListByContainingIngredientIdQuery(request.Id), cancellationToken)    // TODO remove mediatr call, extract query to RecipeRepo, get just count
                .Bind(recipes => Task.FromResult(Result.Ok(new {Ingredient = ingredient, RecipesContainingIngredient = recipes})))
            )
            .Ensure(
                x => x.RecipesContainingIngredient.Count == 0,
                x => IngredientErrors.IngredientIsUsedAndCanNotBeDeletedError(x.RecipesContainingIngredient.Count))
            .Tap(x =>
            {
                repository.Delete(x.Ingredient);
                return Result.Ok();
            }))
            .ToResult();
    }
}
