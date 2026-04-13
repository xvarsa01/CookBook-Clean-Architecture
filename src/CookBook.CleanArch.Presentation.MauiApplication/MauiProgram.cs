using System.Runtime.CompilerServices;
using CommunityToolkit.Maui;
using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using InputKit.Handlers;
using Microsoft.Extensions.DependencyInjection;
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
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddInputKitHandlers();
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var options = GetDALOptions();
        
        builder.Services
            .AddAppServices()
            .AddUseCasesServices()
            .AddInfraServices(options);
        
        var app = builder.Build();
        RegisterRouting(app.Services.GetRequiredService<INavigationService>());
        
        MigrateDb(app.Services.GetRequiredService<IDbMigrator>());
        SeedDb(app.Services.GetRequiredService<IDbSeeder>());
        
        return app;
    }
    
    private static void RegisterRouting(INavigationService navigationService)
    {
        foreach (var route in navigationService.Routes)
        {
            Routing.RegisterRoute(route.Route, route.ViewType);
        }
    }
    
    private static DbOptions GetDALOptions([CallerFilePath] string sourceFilePath = "")
    {
        var relativePath = Path.Combine(Path.GetDirectoryName(sourceFilePath)!,"../CookBook.CleanArch.Infrastructure");
        DbOptions dalOptions = new()
        {
            DatabaseDirectory = Path.GetFullPath(relativePath),
            DatabaseName = "cookbook.db",
        };
        return dalOptions;
    }
    
    private static void MigrateDb(IDbMigrator migrator) => migrator.Migrate();
    private static void SeedDb(IDbSeeder dbSeeder) => dbSeeder.Seed();
}
