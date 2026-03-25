using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Errors;
using CookBook.Clean.Core.IngredientRoot.Events;
using CookBook.Clean.Core.IngredientRoot.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.Commands.Ingredients;

public record UpdateIngredientCommand(IngredientUpdateRequest Request) : ICommand<Guid>;

internal sealed class UpdateIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository, IPublisher publisher)
    : ICommandHandler<UpdateIngredientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var existingIngredient = await repository.GetByIdAsync(request.Request.Id);
        if (existingIngredient == null)
        {
            return Result.NotFound<Guid>(IngredientErrors.IngredientNotFoundError(new IngredientId(request.Request.Id)));
        }

        if (request.Request.Name is not null)
        {
            var result = existingIngredient.UpdateName(request.Request.Name);
            if (result.IsFailure)
                return Result.Invalid<Guid>(result.Error);
        }

        if (request.Request.Description is not null)
        {
            var result = existingIngredient.UpdateDescription(request.Request.Description);
            if (result.IsFailure)
                return Result.Invalid<Guid>(result.Error);
        }

        if (request.Request.ImageUrl is not null)
        {
            var result = existingIngredient.UpdateImageUrl(request.Request.ImageUrl);
            if (result.IsFailure)
                return Result.Invalid<Guid>(result.Error);
        }
        
        var id = await repository.UpdateAsync(existingIngredient);
        if (id is null)
        {
            return Result.Invalid<Guid>(IngredientErrors.IngredientUpdateFailedError(new IngredientId(request.Request.Id)));
        }

        await publisher.Publish(new IngredientUpdatedEvent(existingIngredient), cancellationToken);

        return Result.Ok(id.Value);
    }
}
