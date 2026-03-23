using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Ingredients;

public record CreateIngredientUseCase(string Name, string? Description, string? ImageUrl) : IRequest<Result<Guid>>;

internal class CreateIngredientHandler(IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<CreateIngredientUseCase, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateIngredientUseCase request, CancellationToken cancellationToken) 
    {
        var newIngredientEntity = mapper.MapToEntity(request);
        var createdIngredientId = await repository.InsertAsync(newIngredientEntity);
        return Result<Guid>.Ok(createdIngredientId);
    }
}
