using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Recipe;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.Infrastructure
{
    public class CookBookDbContext(DbContextOptions<CookBookDbContext> options) : DbContext(options)
    {
        public DbSet<IngredientEntity> Ingredients { get; set; } = null!;
        public DbSet<RecipeEntity> Recipes { get; set; } = null!;
        public DbSet<RecipeIngredient> IngredientInRecipe { get; set; } = null!;
    }
}
