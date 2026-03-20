using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Application.ExternalInterfaces;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.Infrastructure.Repositories;

public class EfRecipeRepository : EfRepositoryBase<RecipeEntity>, IRecipeRepository
{
    private readonly DbSet<RecipeEntity> _dbSet;

    public EfRecipeRepository(DbContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<RecipeEntity>();
    }

    public override async Task<RecipeEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(e => e.Ingredients)
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<RecipeEntity>> GetAllContainingIngredientAsync(Guid ingredientId)
    {
        return await _dbSet
            .Where(e => e.Ingredients.Any(i => i.IngredientId == ingredientId))
            .ToListAsync();
    }
}
