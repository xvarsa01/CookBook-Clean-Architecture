using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Ingredients.Queries;

public record GetIngredientDetailQuery(IngredientId Id) : IQuery<IngredientResponse>;

internal class GetIngredientDetailQueryHandler(ICookBookDbContext dbContext) : IQueryHandler<GetIngredientDetailQuery, IngredientResponse>
{
    public async Task<Result<IngredientResponse>> Handle(GetIngredientDetailQuery request, CancellationToken cancellationToken)
    {
        var ingredient = await dbContext
            .Ingredients
            .Where(i => i.Id == request.Id)
            .Select(ingredient => new IngredientResponse(
                ingredient.Id,
                ingredient.Name,
                ingredient.Description,
                ingredient.ImageUrl))
            .FirstOrDefaultAsync(cancellationToken);
        
        return ingredient == null
            ? Result.Failure<IngredientResponse>(IngredientErrors.IngredientNotFoundError(request.Id))
            : Result.Success(ingredient);
    }
}
