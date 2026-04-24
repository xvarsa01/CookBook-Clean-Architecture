using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests;

public class MauiTestsBase : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;

    protected MauiTestsBase()
    {
        var services = new ServiceCollection();

        var dbName = $"{GetType().FullName}_{Guid.NewGuid():N}.db";

        // --- Infra (force in-memory/sqlite test db) ---
        services.AddDbContext<CookBookDbContext>(options =>
        {
            options.UseSqlite($"Data Source={dbName}");
        });

        services.AddScoped<ICookBookDbContext>(sp => sp.GetRequiredService<CookBookDbContext>());
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<CookBookDbContext>());
        
        
        var dbFolder = Path.Combine(Path.GetTempPath(), "CookBookTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(dbFolder);
        
        services.AddInfraServices(new DbOptions
        {
            DatabaseName = dbName,
            UseInMemoryDb = false,
            RecreateDatabaseEachTime = true,
            SeedDemoData = false,
            DatabaseDirectory = dbFolder
        });

        // --- Application layer ---
        services.AddApplicationServices();

        // --- Minimal MAUI service replacements ---
        services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);
        services.AddScoped<INavigationService, TestNavigationService>();
        services.AddScoped<IMessengerService, TestMessengerService>();
        services.AddScoped<IAlertService, TestAlertService>();
        
        services.AddTransient<IngredientDetailViewModel>();
        services.AddTransient<IngredientListViewModel>();
        services.AddTransient<IngredientCreateViewModel>();
        services.AddTransient<IngredientEditViewModel>();
        
        services.AddTransient<RecipeDetailViewModel>();
        services.AddTransient<RecipeListViewModel>();
        services.AddTransient<RecipeCreateViewModel>();
        services.AddTransient<RecipeEditViewModel>();

        services.AddLogging();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected IServiceScope CreateScope() => _serviceProvider.CreateScope();

    public CookBookDbContext GetDbContext(IServiceProvider sp) => sp.GetRequiredService<CookBookDbContext>();
    
    protected IReadOnlyList<Ingredient> SeededIngredients { get; private set; } = [];
    protected IReadOnlyList<Recipe> SeededRecipes { get; private set; } = [];

    public async Task InitializeAsync()
    {
        using var scope = CreateScope();
        var db = GetDbContext(scope.ServiceProvider);

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
        
        SeededIngredients = IngredientTestSeeds.SeededIngredients;
        SeededRecipes = RecipeTestSeeds.SeededRecipes;
        db.AddRange(SeededIngredients);
        db.AddRange(SeededRecipes);
        await db.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        using var scope = CreateScope();
        var db = GetDbContext(scope.ServiceProvider);

        await db.Database.EnsureDeletedAsync();
        await db.DisposeAsync();

        await _serviceProvider.DisposeAsync();
    }
    
    protected async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = CreateScope();
        var sp = scope.ServiceProvider;

        await action(sp);
    }
}
