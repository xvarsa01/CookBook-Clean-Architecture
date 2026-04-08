using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Shells;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication;

public static class Installer
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();

        services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);

        services.AddSingleton<IMessengerService, MessengerService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IAlertService, AlertService>();
        
        services.AddTransient<IngredientDetailViewModel>();
        services.AddTransient<IngredientListViewModel>();
        services.AddTransient<IngredientCreateViewModel>();
        services.AddTransient<IngredientEditViewModel>();
        
        services.AddTransient<RecipeDetailViewModel>();
        services.AddTransient<RecipeListViewModel>();
        services.AddTransient<RecipeEditViewModel>();

        return services;
    }
}
