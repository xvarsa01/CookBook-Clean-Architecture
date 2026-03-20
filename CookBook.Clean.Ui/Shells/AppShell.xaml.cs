using CommunityToolkit.Mvvm.Input;
using CookBook.Clean.Ui.Services;
using CookBook.Clean.Ui.Services.Interfaces;

namespace CookBook.Clean.Ui.Shells;

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
