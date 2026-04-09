using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class EfRecipeRepository(DbContext dbContext) : EfRepository<Recipe, RecipeId>(dbContext)
{
    public override async Task<Recipe?> GetByIdAsync(RecipeId id)
    {
        return await _dbSet
            .Include(r => r.Ingredients)
            .SingleOrDefaultAsync(e => e.Id == id);
    }
}
