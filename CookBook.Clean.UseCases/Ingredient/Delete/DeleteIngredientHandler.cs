using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Ingredient.Events;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Delete;

public record DeleteIngredientResult;

public class DeleteIngredientHandler(IRepository<IngredientEntity> repository, IPublisher publisher)
    : IRequestHandler<DeleteIngredientUseCase , UseCaseResult<DeleteIngredientResult>>
{
    public async Task<UseCaseResult<DeleteIngredientResult>> Handle(DeleteIngredientUseCase request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return UseCaseResult<DeleteIngredientResult>.NotFound("Ingredient not found");
        }
        
        await repository.DeleteAsync(request.Id);

        var ingredientDeletedEvent = new IngredientDeletedEvent(request.Id);
        await publisher.Publish(ingredientDeletedEvent, cancellationToken);

        return UseCaseResult<DeleteIngredientResult>.Ok(new DeleteIngredientResult());
    }
}
