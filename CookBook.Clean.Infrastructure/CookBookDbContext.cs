using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Recipe;
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
                // use shadow foreign key RecipeId to avoid polluting the domain model
                b.WithOwner().HasForeignKey("RecipeId");
                b.Property(i => i.Id).ValueGeneratedNever();
                b.HasKey(i => i.Id);
            });
    }
}
