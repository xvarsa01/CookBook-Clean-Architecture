using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.CleanArch.Infrastructure.Configurations;

public sealed class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new RecipeIngredientId(value)
            );

        builder.HasKey(x => new { x.RecipeId, x.Id });

        builder.Property(x => x.Amount)
            .HasConversion(
                value => value.Value,
                value => IngredientAmount.CreateObject(value).Value
            )
            .IsRequired();

        builder.HasOne(x => x.Ingredient)
            .WithMany()
            .HasForeignKey(x => x.IngredientId);
    }
}

