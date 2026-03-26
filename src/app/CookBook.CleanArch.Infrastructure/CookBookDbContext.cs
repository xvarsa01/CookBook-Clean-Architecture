using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Infrastructure;

public class CookBookDbContext(DbContextOptions<CookBookDbContext> options) : DbContext(options)
{
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<IngredientInRecipe> IngredientInRecipe { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingredient>()
            .Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                value => new IngredientId(value)
            );

        modelBuilder.Entity<Ingredient>()
            .Property(r => r.ImageUrl)
            .HasConversion(
                v => v.Value,                     // string in DB
                v => ImageUrl.CreateObject(v).Value  // convert back to VO
            );
        
        modelBuilder.Entity<Recipe>()
            .Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                value => new RecipeId(value)
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
        
        
        modelBuilder.Entity<IngredientInRecipe>()
            .Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                value => new IngredientInRecipeId(value)
            );
        
        modelBuilder.Entity<IngredientInRecipe>(b =>
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
