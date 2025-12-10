using CookBook.Clean.Core.Ingredient;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Create;

public class CreateIngredientHandler(IRepository<IngredientEntity> repository) : IRequestHandler<CreateIngredientUseCase, Guid>
{
    public async Task<Guid> Handle(CreateIngredientUseCase useCase, CancellationToken cancellationToken) 
    {
        var newIngredient = new IngredientEntity(useCase.Name, useCase.Description, useCase.ImageUrl);
        var createdItemId = await repository.InsertAsync(newIngredient);
        return createdItemId;
    }
}