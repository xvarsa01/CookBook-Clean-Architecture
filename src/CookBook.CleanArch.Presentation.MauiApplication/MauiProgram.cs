using System.Runtime.CompilerServices;
using CommunityToolkit.Maui;
using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using InputKit.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Presentation.MauiApplication;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("icomoon.ttf", "CookBookIcons");
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddInputKitHandlers();
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        builder.Configuration.AddJsonFile(appSettingsPath, optional: true, reloadOnChange: false);

        var options = GetDALOptions(builder.Configuration);
        
        builder.Services
            .AddAppServices()
            .AddApplicationServices()
            .AddInfraServices(options);
        
        var app = builder.Build();
        RegisterRouting(app.Services.GetRequiredService<INavigationService>());
        
        MigrateDb(app.Services.GetRequiredService<IDbMigrator>());

        if (options.SeedDemoData)
        {
            SeedDb(app.Services.GetRequiredService<IDbSeeder>());
        }
        
        return app;
    }
    
    private static void RegisterRouting(INavigationService navigationService)
    {
        foreach (var route in navigationService.Routes)
        {
            Routing.RegisterRoute(route.Route, route.ViewType);
        }
    }
    
    private static DbOptions GetDALOptions(IConfiguration configuration, [CallerFilePath] string sourceFilePath = "")
    {
        var dbSection = configuration.GetSection("CookBook:DB");
        var relativePath = Path.Combine(Path.GetDirectoryName(sourceFilePath)!,"../CookBook.CleanArch.Infrastructure");
        DbOptions dalOptions = new()
        {
            DatabaseDirectory = Path.GetFullPath(relativePath),
            DatabaseName = dbSection["DatabaseName"] ?? "cookbook.db",
            SeedDemoData = TryParseBool(dbSection["SeedDemoData"]),
            RecreateDatabaseEachTime = TryParseBool(dbSection["RecreateDatabaseEachTime"])
        };
        return dalOptions;
    }

    private static bool TryParseBool(string? value) => bool.TryParse(value, out var parsed) && parsed;
    
    private static void MigrateDb(IDbMigrator migrator) => migrator.Migrate();
    private static void SeedDb(IDbSeeder dbSeeder) => dbSeeder.Seed();
}
