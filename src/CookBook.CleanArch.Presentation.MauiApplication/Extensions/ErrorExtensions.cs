using System.Globalization;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApplication.Extensions;

public static class ErrorExtensions
{
    public static string ToLocalizedMessage(this Error error)
    {
        var localizedTemplate = DomainErrorTexts.ResourceManager.GetString(
            error.Code,
            CultureInfo.CurrentUICulture);

        return string.IsNullOrEmpty(localizedTemplate)
            ? error.Message
            : string.Format(localizedTemplate, error.Arguments);
    }
}
