using CookBook.Clean.Core.Ingredient;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Get;

public record GetIngredientResult(IngredientDetailModel Ingredient);

public class GetIngredientHandler(IRepository<IngredientEntity> repository) : IRequestHandler<GetIngredientUseCase, UseCaseResult<GetIngredientResult>>
{
    public async Task<UseCaseResult<GetIngredientResult>> Handle(GetIngredientUseCase request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return UseCaseResult<GetIngredientResult>.NotFound("Ingredient not found");
        }

        return UseCaseResult<GetIngredientResult>.Ok(new GetIngredientResult(
            new IngredientDetailModel { Id = entity.Id, Name = entity.Name, Description = entity.Description, ImageUrl = entity.ImageUrl, }));
    }
}
