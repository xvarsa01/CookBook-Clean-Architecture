using CommunityToolkit.Mvvm.Input;
using CookBook.Clean.App.Services;
using CookBook.Clean.App.Services.Interfaces;

namespace CookBook.Clean.App.Shells;

public partial class AppShell
{
    private readonly INavigationService _navigationService;

    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;

        InitializeComponent();
    }

    [RelayCommand]
    private async Task GoToRecipesAsync()
        => await _navigationService.GoToAsync(NavigationService.RecipeListRouteAbsolute);

    [RelayCommand]
    private async Task GoToIngredientsAsync()
        => await _navigationService.GoToAsync(NavigationService.IngredientListRouteAbsolute);
}
