using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Recipes;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.ExternalInterfaces;

public interface ICookBookDbContext
{
    DbSet<Ingredient> Ingredients { get;}
    DbSet<Recipe> Recipes { get;}
    DbSet<RecipeIngredient> IngredientInRecipe { get;}
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
