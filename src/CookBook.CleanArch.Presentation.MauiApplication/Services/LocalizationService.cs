using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Helpers;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

namespace CookBook.CleanArch.Presentation.MauiApplication.Services;

public class LocalizationService : ILocalizationService
{
    public string[] GetSupportedCultures() => ["en", "cs"];

    public void SetCulture(CultureInfo culture)
    {
        var normalized = NormalizeCulture(new CultureInfo(culture.TwoLetterISOLanguageName));
        
        Thread.CurrentThread.CurrentCulture = normalized;
        Thread.CurrentThread.CurrentUICulture = normalized;

        CultureInfo.DefaultThreadCurrentCulture = normalized;
        CultureInfo.DefaultThreadCurrentUICulture = normalized;

        ApplyResourceCulture(normalized);
        SetDynamicResourcesCulture(normalized);
    }
    
    private void SetDynamicResourcesCulture(CultureInfo culture)
    {
        foreach (var localizationManagerKey in dynamicLocalizationManagerKeys)
        {
            Microsoft.Maui.Controls.Application.Current!.Resources.TryGetValue(localizationManagerKey, out var resource);

            if (resource is DynamicLocalizationManager dynamicLocalizationManager)
            {
                dynamicLocalizationManager.CurrentCulture = culture;
            }
        }
    }
    
    private static void ApplyResourceCulture(CultureInfo culture)
    {
        AppShellTexts.Culture = culture;
        ControlTexts.Culture = culture;
        FoodTypeTexts.Culture = culture;
        IngredientDetailViewModelTexts.Culture = culture;
        IngredientDetailViewTexts.Culture = culture;
        IngredientEditViewTexts.Culture = culture;
        IngredientListViewTexts.Culture = culture;
        LanguageTexts.Culture = culture;
        RecipeCreateViewTexts.Culture = culture;
        RecipeDetailViewTexts.Culture = culture;
        RecipeEditViewTexts.Culture = culture;
        RecipeFormBaseControlTexts.Culture = culture;
        RecipeListViewTexts.Culture = culture;
        SettingsViewTexts.Culture = culture;
        UnitTexts.Culture = culture;
    }
    
    private readonly IEnumerable<string> dynamicLocalizationManagerKeys = new List<string>
    {
        "SettingsLocalizationManager",
        "AppShellLocalizationManager"
    };
        
        
    private static CultureInfo NormalizeCulture(CultureInfo culture)
    {
        return culture.Name switch
        {
            "cs" => new CultureInfo("cs-CZ"),
            "en" => new CultureInfo("en-US"),
            _ => culture
        };
    }

}
