using CookBook.Clean.Core.Ingredient;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Update;

public record UpdateIngredientResult;

public class UpdateIngredientHandler(IRepository<IngredientEntity> repository) : IRequestHandler<UpdateIngredientUseCase, UseCaseResult<UpdateIngredientResult>>
{
    public async Task<UseCaseResult<UpdateIngredientResult>> Handle(UpdateIngredientUseCase request, CancellationToken cancellationToken)
    {
        var existingIngredient = await repository.GetByIdAsync(request.Id);
        if (existingIngredient == null)
        {
            return UseCaseResult<UpdateIngredientResult>.NotFound("Ingredient not found");
        }

        if (request.NewName is not null)
        {
            existingIngredient.UpdateName(request.NewName);
        }

        if (request.NewDescription is not null)
        {
            existingIngredient.UpdateDescription(request.NewDescription);
        }

        if (request.NewImageUrl is not null)
        {
            existingIngredient.UpdateImageUrl(request.NewImageUrl);
        }
        
        await repository.UpdateAsync(existingIngredient);

        return UseCaseResult<UpdateIngredientResult>.Ok(new UpdateIngredientResult());
    }
}