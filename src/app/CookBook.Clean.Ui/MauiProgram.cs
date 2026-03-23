using System.Runtime.CompilerServices;
using CommunityToolkit.Maui;
using CookBook.Clean.Infrastructure;
using CookBook.Clean.Application;
using CookBook.Clean.Ui.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CookBook.Clean.Ui;

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
        var relativePath = Path.Combine(Path.GetDirectoryName(sourceFilePath)!,"../CookBook.Clean.Infrastructure");
        DbOptions dalOptions = new()
        {
            DatabaseDirectory = Path.GetFullPath(relativePath),
            DatabaseName = "cookbook.db",
        };
        return dalOptions;
    }
}
