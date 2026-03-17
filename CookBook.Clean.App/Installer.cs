using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.App.Services;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.App.Shells;
using CookBook.Clean.App.ViewModels;

namespace CookBook.Clean.App;

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
        services.AddTransient<IngredientEditViewModel>();
        services.AddTransient<IngredientListViewModel>();
        
        services.AddTransient<RecipeDetailViewModel>();
        services.AddTransient<RecipeEditViewModel>();
        services.AddTransient<RecipeIngredientsEditViewModel>();
        services.AddTransient<RecipeListViewModel>();

        return services;
    }
}
