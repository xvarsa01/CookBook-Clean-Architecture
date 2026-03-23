using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Events;
using CookBook.Clean.Core.Shared.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Ingredients;


internal class UpdateIngredientHandler(IRepository<IngredientEntity> repository, IPublisher publisher)
    : IRequestHandler<UpdateIngredientUseCase, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateIngredientUseCase request, CancellationToken cancellationToken)
    {
        var existingIngredient = await repository.GetByIdAsync(request.Id);
        if (existingIngredient == null)
        {
            return Result<Guid>.NotFound("Ingredient not found");
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
            existingIngredient.UpdateImageUrl(new ImageUrl(request.NewImageUrl));
        }
        
        var id = await repository.UpdateAsync(existingIngredient);
        if (id is null)
        {
            return Result<Guid>.Invalid("Update failed");
        }

        await publisher.Publish(new IngredientUpdatedEvent(existingIngredient), cancellationToken);

        return Result<Guid>.Ok(id.Value);
    }
}
