using System.Globalization;

namespace CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

public interface ILocalizationService
{
    string[] GetSupportedCultures();
    void SetCulture(CultureInfo culture);
}
