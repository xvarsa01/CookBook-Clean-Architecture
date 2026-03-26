using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient.Errors;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Queries.Ingredients;

public record GetIngredientDetailQuery(IngredientId Id) : IQuery<IngredientDetailResponse>;

internal class GetIngredientDetailQueryHandler(ICookBookDbContext dbContext) : IQueryHandler<GetIngredientDetailQuery, IngredientDetailResponse>
{
    public async Task<Result<IngredientDetailResponse>> Handle(GetIngredientDetailQuery request, CancellationToken cancellationToken)
    {
        var ingredient = await dbContext
            .Ingredients
            .Select(ingredient => new IngredientDetailResponse(
                ingredient.Id,
                ingredient.Name,
                ingredient.Description,
                ingredient.ImageUrl))
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        
        return ingredient == null
            ? Result.NotFound<IngredientDetailResponse>(IngredientErrors.IngredientNotFoundError(request.Id))
            : Result.Ok(ingredient);
    }
}
