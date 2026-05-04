using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Views;
using CookBook.CleanArch.Presentation.MauiApplication.Views.Ingredient;
using CookBook.CleanArch.Presentation.MauiApplication.Views.Recipe;

namespace CookBook.CleanArch.Presentation.MauiApplication.Services;

public class NavigationService : INavigationService
{
    public const string RecipeListRouteAbsolute = "//recipes";
    public const string RecipeDetailRouteRelative = "/detail";
    public const string RecipeEditRouteRelative = "/edit";
    public const string RecipeCreateRouteRelative = "/create";

    public const string IngredientListRouteAbsolute = "//ingredients";
    public const string IngredientDetailRouteRelative = "/detail";
    public const string IngredientEditRouteRelative = "/edit";
    public const string IngredientCreateRouteRelative = "/create";
    
    public const string SettingsAbsolute = "//settings";

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new(IngredientListRouteAbsolute, typeof(IngredientListView)),
        new(IngredientListRouteAbsolute + IngredientDetailRouteRelative, typeof(IngredientDetailView)),
        
        new(IngredientListRouteAbsolute + IngredientCreateRouteRelative, typeof(IngredientCreateView)),
        new(IngredientListRouteAbsolute + IngredientDetailRouteRelative + IngredientEditRouteRelative, typeof(IngredientEditView)),

        new(RecipeListRouteAbsolute, typeof(RecipeListView)),
        new(RecipeListRouteAbsolute + RecipeDetailRouteRelative, typeof(RecipeDetailView)),

        new(RecipeListRouteAbsolute + RecipeCreateRouteRelative, typeof(RecipeCreateView)),
        new(RecipeListRouteAbsolute + RecipeDetailRouteRelative + RecipeEditRouteRelative, typeof(RecipeEditView)),
        
        new(SettingsAbsolute, typeof(SettingsView)),
    };

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();
}
