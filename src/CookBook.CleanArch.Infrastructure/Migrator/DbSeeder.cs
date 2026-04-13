using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Infrastructure.Migrator.SeedData;

namespace CookBook.CleanArch.Infrastructure.Migrator;

public sealed class DbSeeder(CookBookDbContext dbContext) : IDbSeeder
{
    public void Seed()
    {
        var ingredientByName = CreateIngredients();
        var recipes = CreateRecipes(ingredientByName);

        dbContext.AddRange(ingredientByName.Values);
        dbContext.SaveChanges();

        dbContext.AddRange(recipes);
        dbContext.SaveChanges();
    }

    private static Dictionary<string, Ingredient> CreateIngredients()
    {
        var ingredientByName = new Dictionary<string, Ingredient>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in IngredientSeedData.Items)
        {
            var imageUrl = item.imageUrl is null ? null : ImageUrl.CreateObject(item.imageUrl).Value;
            var createResult = Ingredient.Create(item.name, item.description, imageUrl);

            if (createResult.IsFailure)
            {
                throw new InvalidOperationException($"Unable to create ingredient seed '{item.name}'.");
            }

            ingredientByName[item.name] = createResult.Value;
        }

        return ingredientByName;
    }

    private static List<Recipe> CreateRecipes(IReadOnlyDictionary<string, Ingredient> ingredientByName)
    {
        var recipes = new List<Recipe>();

        foreach (var item in RecipeSeedData.Items)
        {
            var createIngredients = item.ingredients
                .Select(seed => new RecipeCreateIngredient(
                    ingredientByName[seed.ingredientName].Id,
                    IngredientAmount.CreateObject((decimal)seed.amount).Value,
                    seed.unit))
                .ToList();

            var name = RecipeName.CreateObject(item.name).Value;
            var imageUrl = item.imageUrl is null ? null : ImageUrl.CreateObject(item.imageUrl).Value;
            var duration = RecipeDuration.CreateObject(item.duration).Value;

            var createResult = Recipe.Create(
                name,
                item.description,
                imageUrl,
                duration,
                item.type,
                createIngredients);

            if (createResult.IsFailure)
            {
                throw new InvalidOperationException($"Unable to create recipe seed '{item.name}'.");
            }

            recipes.Add(createResult.Value);
        }

        return recipes;
    }
}
