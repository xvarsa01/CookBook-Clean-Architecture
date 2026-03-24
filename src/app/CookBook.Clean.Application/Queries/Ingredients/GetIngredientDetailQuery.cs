using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Ingredient;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientDetailQuery(Guid Id) : IQuery<IngredientGetDetailDto>;

internal class GetIngredientDetailQueryHandler(IRepository<IngredientEntity> repository) : IQueryHandler<GetIngredientDetailQuery, IngredientGetDetailDto>
{
    public async Task<Result<IngredientGetDetailDto>> Handle(GetIngredientDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return Result.NotFound<IngredientGetDetailDto>("Ingredient not found");
        }

        var model = new IngredientGetDetailDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl
        };
        return Result.Ok(model);
    }
}
