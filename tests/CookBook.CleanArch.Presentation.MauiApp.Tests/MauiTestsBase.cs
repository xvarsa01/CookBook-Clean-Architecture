using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests;

public class MauiTestsBase : IAsyncLifetime, IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    protected MauiTestsBase()
    {
        var services = new ServiceCollection();

        var dbName = $"{GetType().FullName}_{Guid.NewGuid():N}.db";
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

        // Mock the dispatcher
        var dispatcherMock = new Mock<IDispatcher>();
        var dispatcher = dispatcherMock.Object;
        DispatcherProvider.SetCurrent(new TestDispatcherProvider(dispatcher));
    }

    protected IServiceScope CreateScope() => _serviceProvider.CreateScope();

    public CookBookDbContext GetDbContext(IServiceProvider sp) => sp.GetRequiredService<CookBookDbContext>();

    protected IReadOnlyList<Recipe> SeededRecipes { get; private set; } = RecipeTestSeeds.SeededRecipes;
    
    protected Recipe GetSeededRecipeByName(string recipeName) =>
        SeededRecipes.Single(r => r.Name.Value == recipeName);

    public async Task InitializeAsync()
    {
        using var scope = CreateScope();
        var db = GetDbContext(scope.ServiceProvider);

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
        
        db.AddRange(IngredientTestSeeds.SeededIngredients);
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

    public void Dispose()
    {
        DispatcherProvider.SetCurrent(null);
        GC.SuppressFinalize(this);
    }

    private sealed class TestDispatcherProvider : IDispatcherProvider
    {
        private readonly IDispatcher _dispatcher;

        public TestDispatcherProvider(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public IDispatcher? GetForCurrentThread() => _dispatcher;

        public IDispatcher? GetForMainThread() => _dispatcher;
    }
}
