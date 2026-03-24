using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientDetailQuery(Guid Id) : IQuery<IngredientDetailModel>;

internal class GetIngredientDetailQueryHandler(IRepository<IngredientBase> repository, IIngredientMapper mapper) : IQueryHandler<GetIngredientDetailQuery, IngredientDetailModel>
{
    public async Task<Result<IngredientDetailModel>> Handle(GetIngredientDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return Result.NotFound<IngredientDetailModel>("Ingredient not found");
        }

        var ingredientDetailModel = mapper.MapToDetailModel(entity);
        return Result.Ok<IngredientDetailModel>(ingredientDetailModel);
    }
}
