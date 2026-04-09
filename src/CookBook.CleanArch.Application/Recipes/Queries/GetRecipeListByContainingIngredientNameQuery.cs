using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Recipes.Queries;

public record GetRecipeListByContainingIngredientNameQuery(string IngredientNameSubstring) : IQuery<List<RecipeListResponse>>;

internal class GetRecipeListByContainingIngredientNameQueryHandler (ICookBookDbContext dbContext)
    : IQueryHandler<GetRecipeListByContainingIngredientNameQuery, List<RecipeListResponse>>
{
    public async Task<Result<List<RecipeListResponse>>> Handle(GetRecipeListByContainingIngredientNameQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext
            .Recipes
            .Where(r  => r.Ingredients.Any(recipeIngredient => recipeIngredient.Ingredient.Name.ToLower().Contains(request.IngredientNameSubstring.ToLower())))
            .Select(r => new RecipeListResponse(
                r.Id,
                r.Name,
                r.ImageUrl,
                r.Type))
            .ToListAsync(cancellationToken);
        
        return Result.Ok(list);
    }
}
