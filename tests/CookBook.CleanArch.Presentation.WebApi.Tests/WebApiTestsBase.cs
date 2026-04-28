using System.Text.Json;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes;

namespace CookBook.CleanArch.Presentation.WebApi.Tests;

public abstract class WebApiTestsBase : IAsyncLifetime
{
    protected readonly CookBookApiApplicationFactory Application;
    protected readonly Lazy<HttpClient> Client;
    protected readonly JsonSerializerOptions Options;
    private IReadOnlyList<Recipe> SeededRecipes { get; set; } = [];

    protected Recipe GetSeededRecipeByName(string recipeName) =>
        SeededRecipes.Single(r => r.Name.Value == recipeName);

    protected WebApiTestsBase()
    {
        Application = new CookBookApiApplicationFactory();
        Client = new Lazy<HttpClient>(Application.CreateClient());

        Options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        JsonOptionsSetup.Configure(Options);
    }

    public Task InitializeAsync()
    {
        SeededRecipes = RecipeTestSeeds.SeededRecipes;
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await Application.DisposeAsync();
    }
}


