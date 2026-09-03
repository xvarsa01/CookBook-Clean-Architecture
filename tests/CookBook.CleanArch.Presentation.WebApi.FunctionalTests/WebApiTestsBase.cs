using System.Text.Json;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.CleanArch.Presentation.WebApi.FunctionalTests;

public abstract class WebApiTestsBase : IAsyncLifetime
{
    private readonly CookBookApiApplicationFactory _application = new();

    protected HttpClient Client { get; private set; } = null!;
    protected JsonSerializerOptions Options { get; }
    protected IngredientTestDataSet Ingredients { get; private set; } = null!;
    protected RecipeTestDataSet Recipes { get; private set; } = null!;

    protected WebApiTestsBase()
    {
        Options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        JsonOptionsSetup.Configure(Options);
    }

    public async Task InitializeAsync()
    {
        Client = _application.CreateClient();

        Ingredients = IngredientTestData.CreateSet();
        Recipes = RecipeTestData.CreateSet(Ingredients);

        using var scope = _application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CookBookDbContext>();

        dbContext.AddRange(Ingredients.All);
        dbContext.AddRange(Recipes.All);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        await _application.DisposeAsync();
    }
}
