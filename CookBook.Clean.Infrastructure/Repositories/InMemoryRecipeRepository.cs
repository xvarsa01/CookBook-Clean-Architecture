using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Application;
using CookBook.Clean.Application.ExternalInterfaces;

namespace CookBook.Clean.Infrastructure.Repositories;

public class InMemoryRecipeRepository : InMemoryRepositoryBase<RecipeEntity>, IRecipeRepository
{
    public async Task<List<RecipeEntity>> GetAllContainingIngredientAsync(Guid ingredientId)
    {
        var recipes = await GetAllAsync();
        return recipes
            .Where(r => r.Ingredients.Any(i => i.IngredientId == ingredientId))
            .ToList();
    }
}
