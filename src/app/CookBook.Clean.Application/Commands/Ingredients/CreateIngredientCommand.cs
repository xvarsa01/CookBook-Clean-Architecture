using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.IngredientRoot.ValueObjects;

namespace CookBook.Clean.Application.Commands.Ingredients;

public record CreateIngredientCommand(IngredientCreateRequest Request) : ICommand<Guid>;

internal sealed class CreateIngredientCommandHandler(IRepository<Ingredient, IngredientId> repository) : ICommandHandler<CreateIngredientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateIngredientCommand request, CancellationToken cancellationToken) 
    {
        var result = Ingredient.Create(
            request.Request.Name,
            request.Request.Description,
            request.Request.ImageUrl);
        
        if (result.IsFailure)
            return Result.Invalid<Guid>(result.Error);
        
        var createdIngredientId = await repository.InsertAsync(result.Value);
        return Result.Ok(createdIngredientId);
    }
}
