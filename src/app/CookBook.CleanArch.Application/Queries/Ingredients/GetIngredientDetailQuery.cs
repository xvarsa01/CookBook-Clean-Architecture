using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Ingredient.Errors;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;

namespace CookBook.CleanArch.Application.Queries.Ingredients;

public record GetIngredientDetailQuery(Guid Id) : IQuery<IngredientGetDetailResponse>;

internal class GetIngredientDetailQueryHandler(IRepository<Ingredient, IngredientId> repository) : IQueryHandler<GetIngredientDetailQuery, IngredientGetDetailResponse>
{
    public async Task<Result<IngredientGetDetailResponse>> Handle(GetIngredientDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return Result.NotFound<IngredientGetDetailResponse>(IngredientErrors.IngredientNotFoundError(new IngredientId(request.Id)));
        }

        var model = new IngredientGetDetailResponse(entity.Id, entity.Name, entity.Description, entity.ImageUrl);
        return Result.Ok(model);
    }
}
