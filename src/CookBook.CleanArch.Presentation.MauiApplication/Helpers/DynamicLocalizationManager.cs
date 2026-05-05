using System.Globalization;
using System.Resources;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CookBook.CleanArch.Presentation.MauiApplication.Helpers;

public partial class DynamicLocalizationManager : ObservableObject
{
    [ObservableProperty]
    public partial CultureInfo CurrentCulture { get; set; } = Thread.CurrentThread.CurrentUICulture;
    [ObservableProperty]
    public partial ResourceManager? ResourceManager { get; set; }

    public virtual string this[string text] => GetValue(text);

    public string GetValue(string text)
    {
        var value = ResourceManager?.GetString(text, CurrentCulture);
        return value ?? text;
    }
    
    partial void OnCurrentCultureChanged(
        CultureInfo oldValue,
        CultureInfo newValue)
    {
        OnPropertyChanged(string.Empty);
    }
}
