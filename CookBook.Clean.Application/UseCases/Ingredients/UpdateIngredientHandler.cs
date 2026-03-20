using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Events;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Ingredients;


public class UpdateIngredientHandler(IRepository<IngredientEntity> repository, IPublisher publisher)
    : IRequestHandler<UpdateIngredientUseCase, UseCaseResult<Guid>>
{
    public async Task<UseCaseResult<Guid>> Handle(UpdateIngredientUseCase request, CancellationToken cancellationToken)
    {
        var existingIngredient = await repository.GetByIdAsync(request.Id);
        if (existingIngredient == null)
        {
            return UseCaseResult<Guid>.NotFound("Ingredient not found");
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
        
        var id = await repository.UpdateAsync(existingIngredient);
        if (id is null)
        {
            return UseCaseResult<Guid>.Invalid("Update failed");
        }

        await publisher.Publish(new IngredientUpdatedEvent(existingIngredient), cancellationToken);

        return UseCaseResult<Guid>.Ok(id.Value);
    }
}
