using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.UseCases.Mappers;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Get;

public record GetIngredientResult(IngredientDetailModel Ingredient);

public class GetIngredientHandler(IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<GetIngredientUseCase, UseCaseResult<GetIngredientResult>>
{
    public async Task<UseCaseResult<GetIngredientResult>> Handle(GetIngredientUseCase request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return UseCaseResult<GetIngredientResult>.NotFound("Ingredient not found");
        }

        var ingredientDetailModel = mapper.MapToDetailModel(entity);
        return UseCaseResult<GetIngredientResult>.Ok(new GetIngredientResult(ingredientDetailModel));
    }
}
