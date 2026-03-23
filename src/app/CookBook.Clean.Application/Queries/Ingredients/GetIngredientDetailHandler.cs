using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using MediatR;

namespace CookBook.Clean.Application.Queries.Ingredients;

internal class GetIngredientDetailHandler(IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<GetIngredientDetailQuery, Result<IngredientDetailModel>>
{
    public async Task<Result<IngredientDetailModel>> Handle(GetIngredientDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return Result<IngredientDetailModel>.NotFound("Ingredient not found");
        }

        var ingredientDetailModel = mapper.MapToDetailModel(entity);
        return Result<IngredientDetailModel>.Ok(ingredientDetailModel);
    }
}
