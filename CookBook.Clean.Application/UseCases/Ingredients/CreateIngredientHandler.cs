using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Core.IngredientRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Ingredients;

public class CreateIngredientHandler(IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<CreateIngredientUseCase, UseCaseResult<Guid>>
{
    public async Task<UseCaseResult<Guid>> Handle(CreateIngredientUseCase request, CancellationToken cancellationToken) 
    {
        var newIngredientEntity = mapper.MapToEntity(request);
        var createdIngredientId = await repository.InsertAsync(newIngredientEntity);
        return UseCaseResult<Guid>.Ok(createdIngredientId);
    }
}
