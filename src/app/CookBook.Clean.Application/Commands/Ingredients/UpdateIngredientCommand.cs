using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.Events;
using CookBook.Clean.Core.Shared.ValueObjects;
using MediatR;

namespace CookBook.Clean.Application.Commands.Ingredients;

public record UpdateIngredientCommand(Guid Id, string? NewName, string? NewDescription, string? NewImageUrl) : ICommand<Guid>;

internal sealed class UpdateIngredientCommandHandler(IRepository<IngredientBase> repository, IPublisher publisher)
    : ICommandHandler<UpdateIngredientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var existingIngredient = await repository.GetByIdAsync(request.Id);
        if (existingIngredient == null)
        {
            return Result.NotFound<Guid>("Ingredient not found");
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
            var urlObjectResult = ImageUrl.CreateObject(request.NewImageUrl);
            if (urlObjectResult.IsFailure)
            {
                return Result.Invalid<Guid>(urlObjectResult.Error ?? string.Empty);
            }
            existingIngredient.UpdateImageUrl(urlObjectResult.Value);
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
