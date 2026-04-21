using CookBook.CleanArch.Common.Tests;
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

    public async Task InitializeAsync()
    {
        DbContext = await DbContextFactory.CreateDbContextAsync();
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();

        DbContext.AddRange(RecipeTestSeeds.SeededRecipes);
        DbContext.AddRange(IngredientTestSeeds.SeededIngredients);
        await DbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.DisposeAsync();
        await _serviceProvider.DisposeAsync();
    }
}
