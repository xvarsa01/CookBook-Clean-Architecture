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

internal sealed class DeleteIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository, IRecipeRepository recipeRepository, IPublisher publisher, IMediator mediator)
    : ICommandHandler<DeleteIngredientCommand>
{
    public async Task<Result> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        return (await repository.GetByIdAsync(request.Id)
            .EnsureNotNullNotFound(IngredientErrors.IngredientNotFoundError(request.Id))
            .Bind(ingredient => Task.FromResult(Result.Ok(new
            {
                Ingredient = ingredient,
                RecipesContainingThisIngredientCount = recipeRepository.GetRecipeCountByContainingIngredientId(request.Id)
            })))
            .Ensure(
                x => x.RecipesContainingThisIngredientCount == 0,
                x => IngredientErrors.IngredientIsUsedAndCanNotBeDeletedError(x.RecipesContainingThisIngredientCount))
            .Tap(x => repository.Delete(x.Ingredient)))
            .ToResult();
    }
}
