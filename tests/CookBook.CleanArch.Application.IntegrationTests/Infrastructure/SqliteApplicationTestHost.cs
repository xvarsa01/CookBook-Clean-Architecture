using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.CleanArch.Application.IntegrationTests.Infrastructure;

public sealed class SqliteApplicationTestHost : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ServiceProvider _serviceProvider;

    private SqliteApplicationTestHost(SqliteConnection connection, ServiceProvider serviceProvider)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
    }

    public IServiceProvider Services => _serviceProvider;

    public static async Task<SqliteApplicationTestHost> CreateAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddApplicationServices();
        services.AddDbContext<CookBookDbContext>(options => options.UseSqlite(connection));
        services.AddScoped<ICookBookDbContext>(provider => provider.GetRequiredService<CookBookDbContext>());
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<CookBookDbContext>());
        services.AddScoped<IRecipeRepository, EfRecipeRepository>();

        return new SqliteApplicationTestHost(connection, services.BuildServiceProvider());
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
