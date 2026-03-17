using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.Infrastructure;

public class CookBookDbContext(DbContextOptions<CookBookDbContext> options) : DbContext(options)
{
    public DbSet<IngredientEntity> Ingredients { get; set; } = null!;
    public DbSet<RecipeEntity> Recipes { get; set; } = null!;
    public DbSet<IngredientInRecipeEntity> IngredientInRecipe { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RecipeEntity>()
            .OwnsMany(r => r.Ingredients, b =>
            {
                b.WithOwner().HasForeignKey("RecipeId");
                b.Property(i => i.Id).ValueGeneratedNever();
                b.HasKey(i => i.Id);
            });
    }
}
