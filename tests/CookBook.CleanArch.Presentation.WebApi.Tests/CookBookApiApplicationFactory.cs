using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Infrastructure.Interceptors;
using CookBook.CleanArch.Presentation.WebApi.Tests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CookBook.CleanArch.Presentation.WebApi.Tests;

public sealed class CookBookApiApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");

    public CookBookApiApplicationFactory()
    {
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<CookBookDbContext>>();
            services.AddDbContext<CookBookDbContext>((serviceProvider, options) =>
            {
                options.UseSqlite(_connection);
                options.AddInterceptors(
                    serviceProvider.GetRequiredService<CreatedDateUpdatedDateInterceptor>(),
                    serviceProvider.GetRequiredService<DomainEventsInterceptor>());
            });

            services.RemoveAll<IDbMigrator>();
            services.AddScoped<IDbMigrator, WebApiTestDbMigrator>();

            var dbOptions = new DbOptions
            {
                DatabaseDirectory = "not-used",
                DatabaseName = "not-used.db",
                SeedDemoData = false
            };

            services.RemoveAll<DbOptions>();
            services.RemoveAll<IOptions<DbOptions>>();
            services.AddSingleton(dbOptions);
            services.AddSingleton(Options.Create(dbOptions));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
