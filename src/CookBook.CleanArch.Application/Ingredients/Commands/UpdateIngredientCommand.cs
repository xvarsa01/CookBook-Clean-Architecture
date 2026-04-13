using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Ingredient.Errors;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Commands;

public record UpdateIngredientCommand(IngredientUpdateRequest Request) : ICommand<IngredientId>;

internal sealed class UpdateIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository)
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
        
        return Result.Ok(existingIngredient.Id);
    }
}
