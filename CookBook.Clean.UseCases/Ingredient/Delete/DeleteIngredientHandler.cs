using CookBook.Clean.Core.Ingredient;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Delete;

public record DeleteIngredientResult;

public class DeleteIngredientHandler(IRepository<IngredientEntity> repository) : IRequestHandler<DeleteIngredientUseCase , UseCaseResult<DeleteIngredientResult>>
{
    public async Task<UseCaseResult<DeleteIngredientResult>> Handle(DeleteIngredientUseCase request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return UseCaseResult<DeleteIngredientResult>.Ok(new DeleteIngredientResult());
    }
}