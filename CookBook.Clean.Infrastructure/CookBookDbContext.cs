using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
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
            .Property(r => r.Duration)
            .HasColumnType("time");

        modelBuilder.Entity<RecipeEntity>()
            .Property(r => r.Name)
            .HasConversion(
                v => v.Value,      // store decimal in DB
                v => new RecipeName(v)  // convert back to VO
            );
        
        modelBuilder.Entity<RecipeEntity>()
            .Property(r => r.Duration)
            .HasConversion(
                v => v.Value,      // store decimal in DB
                v => new RecipeDuration(v)  // convert back to VO
            );
        
        
        modelBuilder.Entity<RecipeEntity>()
            .OwnsMany(r => r.Ingredients, b =>
            {
                b.WithOwner().HasForeignKey("RecipeId");
                b.Property(i => i.Id).ValueGeneratedNever();
                b.HasKey(i => i.Id);
                
                b.Property(i => i.Amount)
                    .HasConversion(
                        v => v.Value,         // store decimal in DB
                        v => new IngredientAmount(v)  // convert back to VO
                    )
                    .IsRequired();
                
                b.HasOne<IngredientEntity>()
                    .WithMany()
                    .HasForeignKey(i => i.IngredientId);
            });
    }
}
