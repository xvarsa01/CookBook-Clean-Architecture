using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Infrastructure.Repositories;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Infrastructure;

public sealed class SqliteMauiTestHost : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ServiceProvider _serviceProvider;

    private SqliteMauiTestHost(SqliteConnection connection, ServiceProvider serviceProvider)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
    }

    public IServiceProvider Services => _serviceProvider;

    public static async Task<SqliteMauiTestHost> CreateAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddApplicationServices();
        services.AddAppServices();

        services.AddDbContext<CookBookDbContext>(options => options.UseSqlite(connection));
        services.AddScoped<ICookBookDbContext>(provider => provider.GetRequiredService<CookBookDbContext>());
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<CookBookDbContext>());
        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
        services.AddScoped<IRepository<Recipe, RecipeId>, EfRecipeRepository>();
        services.AddScoped<IRecipeRepository, EfRecipeRepository>();

        services.RemoveAll<IMessenger>();
        services.RemoveAll<INavigationService>();
        services.RemoveAll<IMessengerService>();
        services.RemoveAll<IAlertService>();

        services.AddSingleton<IMessenger>(_ => new WeakReferenceMessenger());
        services.AddScoped<INavigationService, TestNavigationService>();
        services.AddScoped<IMessengerService, TestMessengerService>();
        services.AddScoped<IAlertService, TestAlertService>();

        var dispatcher = new Mock<IDispatcher>().Object;
        DispatcherProvider.SetCurrent(new TestDispatcherProvider(dispatcher));

        return new SqliteMauiTestHost(connection, services.BuildServiceProvider());
    }

    public async ValueTask DisposeAsync()
    {
        DispatcherProvider.SetCurrent(null);
        await _serviceProvider.DisposeAsync();
        await _connection.DisposeAsync();
    }

    private sealed class TestDispatcherProvider(IDispatcher dispatcher) : IDispatcherProvider
    {
        public IDispatcher? GetForCurrentThread() => dispatcher;

        public IDispatcher? GetForMainThread() => dispatcher;
    }
}
