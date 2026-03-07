using CookBook.Clean.Core.Recipe;
using CookBook.Clean.UseCases.Mappers;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Create;

public record CreateRecipeResult(Guid Id);

public class CreateRecipeHandler(IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<CreateRecipeUseCase, UseCaseResult<CreateRecipeResult>>
{
    public async Task<UseCaseResult<CreateRecipeResult>> Handle(CreateRecipeUseCase request, CancellationToken cancellationToken)
    {
        var newRecipe = mapper.MapToEntity(request);
        var createdItemId = await repository.InsertAsync(newRecipe);
        return UseCaseResult<CreateRecipeResult>.Ok(new CreateRecipeResult(createdItemId));
    }
}
