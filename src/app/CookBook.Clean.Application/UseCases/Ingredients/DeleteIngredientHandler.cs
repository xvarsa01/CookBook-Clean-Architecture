using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Events;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Ingredients;

internal class DeleteIngredientHandler(IRepository<IngredientEntity> repository, IRecipeRepository recipeRepository, IPublisher publisher)
    : IRequestHandler<DeleteIngredientUseCase , UseCaseResult>
{
    public async Task<UseCaseResult> Handle(DeleteIngredientUseCase request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return UseCaseResult.NotFound("Ingredient not found");
        }
        
        var recipesContainingIngredient = await recipeRepository.GetAllContainingIngredientAsync(request.Id);

        if (recipesContainingIngredient.Count != 0)
        {
            return UseCaseResult.Invalid("Cannot delete ingredient that is used in recipes. Remove it from all recipes first.");
        }
        
        await repository.DeleteAsync(request.Id);

        var ingredientDeletedEvent = new IngredientDeletedEvent(request.Id);
        await publisher.Publish(ingredientDeletedEvent, cancellationToken);

        return UseCaseResult.Ok();
    }
}
