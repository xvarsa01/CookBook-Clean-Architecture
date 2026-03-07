using CookBook.Clean.Core.Recipe;
using CookBook.Clean.UseCases;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.Infrastructure.Repositories;

public class EfRecipeRepository : EfRepositoryBase<RecipeEntity>, IRecipeRepository
{
    private readonly DbSet<RecipeEntity> _dbSet;

    public EfRecipeRepository(DbContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<RecipeEntity>();
    }

    public async Task<List<RecipeEntity>> GetAllContainingIngredientAsync(Guid ingredientId)
    {
        return await _dbSet
            .Where(e => e.Ingredients.Any(i => i.IngredientId == ingredientId))
            .ToListAsync();
    }
}
