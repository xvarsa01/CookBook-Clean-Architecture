using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Commands.Ingredients;

public record CreateIngredientCommand(IngredientCreateDto Dto) : ICommand<Guid>;

internal sealed class CreateIngredientCommandHandler(IRepository<Ingredient> repository) : ICommandHandler<CreateIngredientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateIngredientCommand request, CancellationToken cancellationToken) 
    {
        var result = Ingredient.Create(
            request.Dto.Name,
            request.Dto.Description,
            request.Dto.ImageUrl);
        
        if (result.IsFailure)
            return Result.Invalid<Guid>(result.Error);
        
        var createdIngredientId = await repository.InsertAsync(result.Value);
        return Result.Ok(createdIngredientId);
    }
}
