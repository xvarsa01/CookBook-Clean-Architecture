using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class EfRecipeRepository(DbContext dbContext) : EfRepository<Recipe, RecipeId>(dbContext), IRecipeRepository
{
    public override async Task<Recipe?> GetByIdAsync(RecipeId id)
    {
        return await _dbSet
            .Include(r => r.Ingredients)
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public int GetRecipeCountByContainingIngredientId(IngredientId ingredientId)
    {
        return _dbSet.Count(recipe => recipe.Ingredients.Any(ri => ri.IngredientId == ingredientId));
    }
    
    public async Task<Recipe?> GetRecipeWithIngredientsByIdAsync(RecipeId id)// this is not used in current application, check `GetRecipeDetailQuery` class for explanation
    {
        return await _dbSet
            .Include(r => r.Ingredients)
            .ThenInclude(i => i.Ingredient)
            .SingleOrDefaultAsync(e => e.Id == id);
    }
}
