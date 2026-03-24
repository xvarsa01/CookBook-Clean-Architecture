using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;
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

        modelBuilder.Entity<IngredientEntity>()
            .Property(r => r.ImageUrl)
            .HasConversion(
                v => v.Value,                     // string in DB
                v => ImageUrl.CreateObject(v).Value  // convert back to VO
            );
        
        modelBuilder.Entity<RecipeEntity>()
            .Property(r => r.ImageUrl)
            .HasConversion(
                v => v.Value,                     // string in DB
                v => ImageUrl.CreateObject(v).Value  // convert back to VO
            );

        modelBuilder.Entity<RecipeEntity>()
            .Property(r => r.Name)
            .HasConversion(
                v => v.Value,                     // string in DB
                v => RecipeName.CreateObject(v).Value  // convert back to VO
            );
        
        modelBuilder.Entity<RecipeEntity>()
            .Property(r => r.Duration)
            .HasColumnType("INTEGER")
            .HasConversion(
                v => v.Value.TotalSeconds,                                           // int in DB
                v => RecipeDuration.CreateObject(TimeSpan.FromSeconds(v)).Value  // convert back to VO
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
                        v => IngredientAmount.CreateObject(v).Value  // convert back to VO
                    )
                    .IsRequired();
                
                b.HasOne<IngredientEntity>()
                    .WithMany()
                    .HasForeignKey(i => i.IngredientId);
            });
    }
}
