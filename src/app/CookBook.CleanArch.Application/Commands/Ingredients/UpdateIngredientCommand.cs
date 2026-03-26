using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Ingredient.Errors;
using CookBook.CleanArch.Domain.Ingredient.Events;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using MediatR;

namespace CookBook.CleanArch.Application.Commands.Ingredients;

public record UpdateIngredientCommand(IngredientUpdateRequest Request) : ICommand<IngredientId>;

internal sealed class UpdateIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository, IPublisher publisher)
    : ICommandHandler<UpdateIngredientCommand, IngredientId>
{
    public async Task<Result<IngredientId>> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var existingIngredient = await repository.GetByIdAsync(request.Request.Id);
        if (existingIngredient == null)
        {
            return Result.NotFound<IngredientId>(IngredientErrors.IngredientNotFoundError(new IngredientId(request.Request.Id)));
        }

        if (request.Request.Name is not null)
        {
            var result = existingIngredient.UpdateName(request.Request.Name);
            if (result.IsFailure)
                return Result.Invalid<IngredientId>(result.Error);
        }

        if (request.Request.Description is not null)
        {
            var result = existingIngredient.UpdateDescription(request.Request.Description);
            if (result.IsFailure)
                return Result.Invalid<IngredientId>(result.Error);
        }

        if (request.Request.ImageUrl is not null)
        {
            var result = existingIngredient.UpdateImageUrl(request.Request.ImageUrl);
            if (result.IsFailure)
                return Result.Invalid<IngredientId>(result.Error);
        }
        
        var id = await repository.UpdateAsync(existingIngredient);
        if (id is null)
        {
            return Result.Invalid<IngredientId>(IngredientErrors.IngredientUpdateFailedError(new IngredientId(request.Request.Id)));
        }

        await publisher.Publish(new IngredientUpdatedEvent(existingIngredient), cancellationToken);

        return Result.Ok(id);
    }
}
