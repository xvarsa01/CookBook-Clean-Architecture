using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Presentation.MauiApp.IntegrationTests.Infrastructure;

namespace CookBook.CleanArch.Presentation.MauiApp.IntegrationTests;

public abstract class MauiTestsBase : IAsyncLifetime
{
    private SqliteMauiTestHost _testHost = null!;

    protected IngredientTestDataSet Ingredients { get; private set; } = null!;
    protected RecipeTestDataSet Recipes { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _testHost = await SqliteMauiTestHost.CreateAsync();

        await ExecuteScopeAsync(async services =>
        {
            var dbContext = GetDbContext(services);
            await dbContext.Database.EnsureCreatedAsync();

            Ingredients = IngredientTestData.CreateSet();
            Recipes = RecipeTestData.CreateSet(Ingredients);

            dbContext.AddRange(Ingredients.All);
            dbContext.AddRange(Recipes.All);
            await dbContext.SaveChangesAsync();
        });
    }

    public async Task DisposeAsync() => await _testHost.DisposeAsync();

    protected async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _testHost.Services.CreateScope();
        await action(scope.ServiceProvider);
    }

    protected static CookBookDbContext GetDbContext(IServiceProvider services) =>
        services.GetRequiredService<CookBookDbContext>();
}
