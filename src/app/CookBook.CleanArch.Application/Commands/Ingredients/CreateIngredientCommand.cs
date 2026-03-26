using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;

namespace CookBook.CleanArch.Application.Commands.Ingredients;

public record CreateIngredientCommand(IngredientCreateRequest Request) : ICommand<IngredientId>;

internal sealed class CreateIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository) : ICommandHandler<CreateIngredientCommand, IngredientId>
{
    public async Task<Result<IngredientId>> Handle(CreateIngredientCommand request, CancellationToken cancellationToken) 
    {
        var result = Ingredient.Create(
            request.Request.Name,
            request.Request.Description,
            request.Request.ImageUrl);
        
        if (result.IsFailure)
            return Result.Invalid<IngredientId>(result.Error);
        
        var createdIngredientId = await repository.InsertAsync(result.Value);
        return Result.Ok(createdIngredientId);
    }
}
