using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Ingredient.Events;
using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Delete;

public record DeleteIngredientResult;

public class DeleteIngredientHandler(IRepository<IngredientEntity> repository, IRecipeRepository recipeRepository, IPublisher publisher)
    : IRequestHandler<DeleteIngredientUseCase , UseCaseResult<DeleteIngredientResult>>
{
    public async Task<UseCaseResult<DeleteIngredientResult>> Handle(DeleteIngredientUseCase request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return UseCaseResult<DeleteIngredientResult>.NotFound("Ingredient not found");
        }
        
        var recipesContainingIngredient = await recipeRepository.GetAllContainingIngredientAsync(request.Id);

        if (recipesContainingIngredient.Count != 0)
        {
            return UseCaseResult<DeleteIngredientResult>.Invalid("Cannot delete ingredient that is used in recipes. Remove it from all recipes first.");
        }
        
        await repository.DeleteAsync(request.Id);

        var ingredientDeletedEvent = new IngredientDeletedEvent(request.Id);
        await publisher.Publish(ingredientDeletedEvent, cancellationToken);

        return UseCaseResult<DeleteIngredientResult>.Ok(new DeleteIngredientResult());
    }
}
