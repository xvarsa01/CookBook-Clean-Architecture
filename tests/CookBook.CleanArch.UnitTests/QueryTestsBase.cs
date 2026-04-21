using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Infrastructure.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.CleanArch.UnitTests;

public class QueryTestsBase : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;

    protected QueryTestsBase()
    {
        var databaseName = $"{GetType().FullName}_{Guid.NewGuid():N}.db";
        DbContextFactory = new DbContextSqLiteFactory(databaseName);

        var services = new ServiceCollection();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected IDbContextFactory<CookBookDbContext> DbContextFactory { get; }
    protected CookBookDbContext DbContext { get; private set; } = null!;
    protected IReadOnlyList<Ingredient> SeededIngredients { get; private set; } = [];
    protected IReadOnlyList<Recipe> SeededRecipes { get; private set; } = [];

    protected Ingredient GetSeededIngredientsByName(string ingredientName) =>
        SeededIngredients.Single(r => r.Name == ingredientName);
    protected Recipe GetSeededRecipeByName(string recipeName) =>
        SeededRecipes.Single(r => r.Name.Value == recipeName);

    public async Task InitializeAsync()
    {
        DbContext = await DbContextFactory.CreateDbContextAsync();
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();

        SeededIngredients = IngredientTestSeeds.SeededIngredients;
        SeededRecipes = RecipeTestSeeds.SeededRecipes;
        DbContext.AddRange(SeededIngredients);
        DbContext.AddRange(SeededRecipes);
        await DbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.DisposeAsync();
        await _serviceProvider.DisposeAsync();
    }
}
