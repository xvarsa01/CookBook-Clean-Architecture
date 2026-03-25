using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Events;
using CookBook.Clean.Core.IngredientRoot.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.Commands.Ingredients;

public record UpdateIngredientCommand(IngredientUpdateRequest Model) : ICommand<Guid>;

internal sealed class UpdateIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository, IPublisher publisher)
    : ICommandHandler<UpdateIngredientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var existingIngredient = await repository.GetByIdAsync(request.Model.Id);
        if (existingIngredient == null)
        {
            return Result.NotFound<Guid>("Ingredient not found");
        }

        if (request.Model.Name is not null)
        {
            var result = existingIngredient.UpdateName(request.Model.Name);
            if (result.IsFailure)
                return Result.Invalid<Guid>(result.Error);
        }

        if (request.Model.Description is not null)
        {
            var result = existingIngredient.UpdateDescription(request.Model.Description);
            if (result.IsFailure)
                return Result.Invalid<Guid>(result.Error);
        }

        if (request.Model.ImageUrl is not null)
        {
            var result = existingIngredient.UpdateImageUrl(request.Model.ImageUrl);
            if (result.IsFailure)
                return Result.Invalid<Guid>(result.Error);
        }
        
        var id = await repository.UpdateAsync(existingIngredient);
        if (id is null)
        {
            return Result.Invalid<Guid>("Update failed");
        }

        await publisher.Publish(new IngredientUpdatedEvent(existingIngredient), cancellationToken);

        return Result.Ok(id.Value);
    }
}
