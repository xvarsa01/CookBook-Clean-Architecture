using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;

namespace CookBook.CleanArch.Application.Ingredients.Commands;

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
        
        var createdIngredientId = repository.Add(result.Value);
        return Result.Ok(createdIngredientId);
    }
}
