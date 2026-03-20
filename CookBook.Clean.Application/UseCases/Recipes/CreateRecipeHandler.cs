using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public class CreateRecipeHandler(IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<CreateRecipeUseCase, UseCaseResult<Guid>>
{
    public async Task<UseCaseResult<Guid>> Handle(CreateRecipeUseCase request, CancellationToken cancellationToken)
    {
        var newRecipe = mapper.MapToEntity(request);
        var createdItemId = await repository.InsertAsync(newRecipe);
        return UseCaseResult<Guid>.Ok(createdItemId);
    }
}
