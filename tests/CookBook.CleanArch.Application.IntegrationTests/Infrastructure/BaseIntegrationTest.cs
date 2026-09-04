using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.CleanArch.Application.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest : IAsyncLifetime
{
    private SqliteApplicationTestHost _testHost = null!;
    private IServiceScope _scope = null!;

    protected CookBookDbContext DbContext { get; private set; } = null!;
    protected IMediator Mediator { get; private set; } = null!;
    protected IngredientTestDataSet Ingredients { get; private set; } = null!;
    protected RecipeTestDataSet Recipes { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _testHost = await SqliteApplicationTestHost.CreateAsync();

        _scope = _testHost.Services.CreateScope();
        var scopedProvider = _scope.ServiceProvider;

        DbContext = scopedProvider.GetRequiredService<CookBookDbContext>();
        Mediator = scopedProvider.GetRequiredService<IMediator>();

        await DbContext.Database.EnsureCreatedAsync();

        Ingredients = IngredientTestData.CreateSet();
        Recipes = RecipeTestData.CreateSet(Ingredients);
        DbContext.AddRange(Ingredients.All);
        DbContext.AddRange(Recipes.All);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _testHost.DisposeAsync();
    }
}
