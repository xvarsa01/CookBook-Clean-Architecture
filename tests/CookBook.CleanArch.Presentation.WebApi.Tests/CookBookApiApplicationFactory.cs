using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CookBook.CleanArch.Presentation.WebApi.Tests;

public class CookBookApiApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string databaseDirectory = Path.Combine(
        Path.GetTempPath(),
        "CookBookTests",
        $"{nameof(CookBookApiApplicationFactory)}_{Guid.NewGuid():N}");

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(collection =>
        {
            collection.AddMvc().AddApplicationPart(typeof(Program).Assembly);
        });
        return base.CreateHost(builder);
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            Directory.CreateDirectory(databaseDirectory);

            var dict = new Dictionary<string, string?>
            {
                ["CookBook:DB:DatabaseDirectory"] = databaseDirectory,
                ["CookBook:DB:DatabaseName"] = "recipe-controller-tests.db",
                ["CookBook:DB:RecreateDatabaseEachTime"] = "true",
                ["CookBook:DB:SeedDemoData"] = "false",
                ["CookBook:DB:UseInMemoryDatabase"] = "false"
            };

            config.AddInMemoryCollection(dict);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbOptions>();
            services.AddSingleton(new DbOptions
            {
                DatabaseDirectory = databaseDirectory,
                DatabaseName = "recipe-controller-tests.db",
                RecreateDatabaseEachTime = true,
                SeedDemoData = true,
                UseInMemoryDb = false
            });

            // Replace the default DbSeeder with test-specific seeder
            services.RemoveAll<IDbSeeder>();
            services.AddScoped<IDbSeeder, WebApiTestDbSeeder>();
        });
    }
}
