using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Infrastructure;

namespace CookBook.CleanArch.Presentation.WebApi.Tests;

public class WebApiTestDbSeeder(CookBookDbContext dbContext) : IDbSeeder
{
    public void Seed()
    {
        if (dbContext.Ingredients.Any() || dbContext.Recipes.Any())
        {
            return;
        }

        dbContext.AddRange(IngredientTestSeeds.SeededIngredients);
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();

        dbContext.AddRange(RecipeTestSeeds.SeededRecipes);
        dbContext.SaveChanges();
    }
}

