using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CookBook.CleanArch.Presentation.MauiApplication.Helpers;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class SettingsViewModel(
    IMessengerService messengerService,
    ILocalizationService localizationService) : ViewModelBase(messengerService)
{
    [ObservableProperty]
    public partial ObservableCollection<LanguageSelectionModel> Languages { get; set; } = new();
    
    [ObservableProperty]
    public partial LanguageSelectionModel? SelectedLanguage { get; set; }

    [ObservableProperty]
    public partial bool IsDarkTheme { get; set; }
    
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        LoadLanguageSettings();
        LoadThemeSettings();
    }

    private void LoadLanguageSettings()
    {
        Languages = [];
        foreach (var culture in localizationService.GetSupportedCultures())
        {
            Languages.Add(new LanguageSelectionModel
            {
                Culture = culture
            });
        }

        SelectedLanguage ??= Languages.Single(language => language.Culture == "en");
    }

    private void LoadThemeSettings()
    {
        var app = global::Microsoft.Maui.Controls.Application.Current;
        IsDarkTheme = app?.UserAppTheme == AppTheme.Dark;
    }
    
    partial void OnSelectedLanguageChanged(LanguageSelectionModel? value)
    {
        if (string.IsNullOrWhiteSpace(SelectedLanguage?.Culture))
        {
            return;
        }

        localizationService.SetCulture(new CultureInfo(SelectedLanguage.Culture));
    }
    
    partial void OnIsDarkThemeChanged(bool value)
    {
        var app = Microsoft.Maui.Controls.Application.Current;

        app?.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
        ThemeResourceManager.ApplyThemeResources(app?.UserAppTheme ?? AppTheme.Dark);
    }
}
