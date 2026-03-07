using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Create;

public record CreateRecipeResult(Guid Id);

public class CreateRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<CreateRecipeUseCase, UseCaseResult<CreateRecipeResult>>
{
    public async Task<UseCaseResult<CreateRecipeResult>> Handle(CreateRecipeUseCase request, CancellationToken cancellationToken)
    {
        var newRecipe = new RecipeEntity(request.Name, request.Description, request.ImageUrl, request.RecipeType);
        var createdItemId = await repository.InsertAsync(newRecipe);
        return UseCaseResult<CreateRecipeResult>.Ok(new CreateRecipeResult(createdItemId));
    }
}
