using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Create;

public class CreateIngredientHandler(IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<CreateIngredientUseCase, UseCaseResult<Guid>>
{
    public async Task<UseCaseResult<Guid>> Handle(CreateIngredientUseCase request, CancellationToken cancellationToken) 
    {
        var newIngredientEntity = mapper.MapToEntity(request);
        var createdIngredientId = await repository.InsertAsync(newIngredientEntity);
        return UseCaseResult<Guid>.Ok(createdIngredientId);
    }
}
