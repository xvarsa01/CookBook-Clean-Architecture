using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientIdQuery(IngredientId IngredientId) : IQuery<List<RecipeListResponse>>;

internal class GetRecipeListByContainingIngredientIdQueryHandler (ICookBookDbContext dbContext) : IQueryHandler<GetRecipeListByContainingIngredientIdQuery, List<RecipeListResponse>>
{
    public async Task<Result<List<RecipeListResponse>>> Handle(GetRecipeListByContainingIngredientIdQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Recipes.Where(recipe => recipe.Ingredients.Any(ri => ri.IngredientId == request.IngredientId))
            .Select(r => new RecipeListResponse(
                r.Id,
                r.Name,
                r.ImageUrl))
            .ToListAsync(cancellationToken);
        
        return (Result.Ok(list));
    }
}
