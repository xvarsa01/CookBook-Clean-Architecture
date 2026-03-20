using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;
using MediatR;

namespace CookBook.Clean.Application.Queries.Ingredients;

public class GetIngredientDetailHandler(IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<GetIngredientDetailQuery, UseCaseResult<IngredientDetailModel>>
{
    public async Task<UseCaseResult<IngredientDetailModel>> Handle(GetIngredientDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return UseCaseResult<IngredientDetailModel>.NotFound("Ingredient not found");
        }

        var ingredientDetailModel = mapper.MapToDetailModel(entity);
        return UseCaseResult<IngredientDetailModel>.Ok(ingredientDetailModel);
    }
}
