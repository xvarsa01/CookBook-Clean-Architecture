using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.CleanArch.Infrastructure.Configurations;

public sealed class RecipeReviewConfiguration : IEntityTypeConfiguration<RecipeReview>
{
    public void Configure(EntityTypeBuilder<RecipeReview> builder)
    {
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new RecipeReviewId(value)
            );

        builder.HasKey(x => new { x.RecipeId, x.Id });

        builder.Property(x => x.Mark)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(Recipe.MaxReviewDescriptionLength)
            .IsRequired();
    }
}
