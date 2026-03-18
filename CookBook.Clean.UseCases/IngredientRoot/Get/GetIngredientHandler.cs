using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.Get;

public class GetIngredientHandler(IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<GetIngredientUseCase, UseCaseResult<IngredientDetailModel>>
{
    public async Task<UseCaseResult<IngredientDetailModel>> Handle(GetIngredientUseCase request, CancellationToken cancellationToken)
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
