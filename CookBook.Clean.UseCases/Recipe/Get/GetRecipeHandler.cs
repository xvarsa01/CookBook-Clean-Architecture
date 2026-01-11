using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Get;

public record GetRecipeResult(RecipeDetailModel Recipe);

public class GetRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<GetRecipeUseCase, UseCaseResult<GetRecipeResult>>
{
    public async Task<UseCaseResult<GetRecipeResult>> Handle(GetRecipeUseCase request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return UseCaseResult<GetRecipeResult>.NotFound("Recipe not found");
        }

        return UseCaseResult<GetRecipeResult>.Ok(new GetRecipeResult(
            new RecipeDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                Ingredients = entity.Ingredients.Select(i => new RecipeIngredientModel
                {
                    Id = i.Id,
                    IngredientId = i.IngredientId,
                    Amount = i.Amount,
                    Unit = i.Unit
                }).ToList(),
            }));
    }
}
