using CookBook.CleanArch.Domain.IngredientRoot;
using CookBook.CleanArch.Domain.RecipeRoot;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Infrastructure;

public class CookBookDbContext(DbContextOptions<CookBookDbContext> options) : DbContext(options)
{
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<IngredientInRecipeEntity> IngredientInRecipe { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingredient>()
            .Property(r => r.ImageUrl)
            .HasConversion(
                v => v.Value,                     // string in DB
                v => ImageUrl.CreateObject(v).Value  // convert back to VO
            );
        
        modelBuilder.Entity<Recipe>()
            .Property(r => r.ImageUrl)
            .HasConversion(
                v => v.Value,                     // string in DB
                v => ImageUrl.CreateObject(v).Value  // convert back to VO
            );

        modelBuilder.Entity<Recipe>()
            .Property(r => r.Name)
            .HasConversion(
                v => v.Value,                     // string in DB
                v => RecipeName.CreateObject(v).Value  // convert back to VO
            );
        
        modelBuilder.Entity<Recipe>()
            .Property(r => r.Duration)
            .HasColumnType("INTEGER")
            .HasConversion(
                v => v.Value.TotalSeconds,                                           // int in DB
                v => RecipeDuration.CreateObject(TimeSpan.FromSeconds(v)).Value  // convert back to VO
            );
        
        
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Ingredients)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<IngredientInRecipeEntity>(b =>
        {
            b.HasKey(i => new { i.RecipeId, i.Id });
            
            b.Property(i => i.Amount)
                .HasConversion(
                    v => v.Value,
                    v => IngredientAmount.CreateObject(v).Value
                )
                .IsRequired();

            b.HasOne<Ingredient>()
                .WithMany()
                .HasForeignKey(i => i.IngredientId);
        });
    }
}
