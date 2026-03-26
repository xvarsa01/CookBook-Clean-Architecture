using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientNameQuery(string IngredientNameSubstring) : IQuery<List<RecipeGetListResponse>>;

internal class GetRecipeListByContainingIngredientNameQueryHandler (ICookBookDbContext dbContext)
    : IQueryHandler<GetRecipeListByContainingIngredientNameQuery, List<RecipeGetListResponse>>
{
    public async Task<Result<List<RecipeGetListResponse>>> Handle(GetRecipeListByContainingIngredientNameQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext
            .Recipes
            .Where(r  => r.Ingredients.Any(ri => ri.Ingredient.Name.ToLower().Contains(request.IngredientNameSubstring.ToLower())))
            .Select(i => new RecipeGetListResponse(
                i.Id,
                i.Name,
                i.ImageUrl))
            .ToListAsync(cancellationToken);
        
        return Result.Ok(list);
    }
}
