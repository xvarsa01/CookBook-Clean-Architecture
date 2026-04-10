using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.CleanArch.Infrastructure.Configurations;

public sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new RecipeId(value)
            );

        builder.Property(x => x.ImageUrl)
            .HasConversion(
                value => value!.Value,
                value => ImageUrl.CreateObject(value).Value
            );

        builder.Property(x => x.Name)
            .HasConversion(
                value => value.Value,
                value => RecipeName.CreateObject(value).Value
            );

        builder.Property(x => x.Duration)
            .HasColumnType("INTEGER")
            .HasConversion(
                value => value.Value.TotalSeconds,
                value => RecipeDuration.CreateObject(TimeSpan.FromSeconds(value)).Value
            );

        builder.HasMany(x => x.Ingredients)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}



