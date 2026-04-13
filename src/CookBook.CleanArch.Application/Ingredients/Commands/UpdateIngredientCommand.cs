using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Commands;

public record UpdateIngredientCommand(IngredientUpdateRequest Request) : ICommand<IngredientId>;

internal sealed class UpdateIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository)
    : ICommandHandler<UpdateIngredientCommand, IngredientId>
{
    public async Task<Result<IngredientId>> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.GetByIdAsync(request.Request.Id)
            .EnsureNotNullNotFound(IngredientErrors.IngredientNotFoundError(new IngredientId(request.Request.Id)))
            .Tap(ingredient => request.Request.Name is null
                ? Result.Ok()
                : ingredient.UpdateName(request.Request.Name))
            .Tap(ingredient => request.Request.Description is null
                ? Result.Ok()
                : ingredient.UpdateDescription(request.Request.Description))
            .Tap(ingredient => request.Request.ImageUrl is null
                ? Result.Ok()
                : ingredient.UpdateImageUrl(request.Request.ImageUrl));

        return result.Map(ingredient => ingredient.Id);
    }
}
