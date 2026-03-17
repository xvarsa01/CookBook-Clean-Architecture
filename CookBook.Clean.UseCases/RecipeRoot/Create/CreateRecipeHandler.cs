using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Create;

public class CreateRecipeHandler(IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<CreateRecipeUseCase, UseCaseResult<Guid>>
{
    public async Task<UseCaseResult<Guid>> Handle(CreateRecipeUseCase request, CancellationToken cancellationToken)
    {
        var newRecipe = mapper.MapToEntity(request);
        var createdItemId = await repository.InsertAsync(newRecipe);
        return UseCaseResult<Guid>.Ok(createdItemId);
    }
}
