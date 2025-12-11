using CookBook.Clean.Core.Ingredient;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Create;

public class CreateIngredientHandler(IRepository<IngredientEntity> repository) : IRequestHandler<CreateIngredientUseCase, Guid>
{
    public async Task<Guid> Handle(CreateIngredientUseCase request, CancellationToken cancellationToken) 
    {
        var newIngredient = new IngredientEntity(request.Name, request.Description, request.ImageUrl);
        var createdItemId = await repository.InsertAsync(newIngredient);
        return createdItemId;
    }
}
