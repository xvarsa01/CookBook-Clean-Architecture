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
        await repository.DeleteAsync(request.Id);

        await publisher.Publish(new IngredientDeletedEvent(request.Id), cancellationToken);

        return UseCaseResult<DeleteIngredientResult>.Ok(new DeleteIngredientResult());
    }
}
