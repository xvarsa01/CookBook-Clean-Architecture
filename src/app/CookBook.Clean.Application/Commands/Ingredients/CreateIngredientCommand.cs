using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.UseCases.Ingredients;

public record CreateIngredientCommand(string Name, string? Description, string? ImageUrl) : ICommand<Guid>;

internal sealed class CreateIngredientCommandHandler(IRepository<IngredientEntity> repository, IIngredientMapper mapper) : ICommandHandler<CreateIngredientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateIngredientCommand request, CancellationToken cancellationToken) 
    {
        var newIngredientEntity = mapper.MapToEntity(request);
        var createdIngredientId = await repository.InsertAsync(newIngredientEntity);
        return Result.Ok(createdIngredientId);
    }
}
