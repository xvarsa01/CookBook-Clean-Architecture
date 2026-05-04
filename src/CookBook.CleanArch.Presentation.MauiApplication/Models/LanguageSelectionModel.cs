using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApplication.Models;

public class LanguageSelectionModel
{
    public required string Culture { get; set; }
    public string Text => LanguageTexts.ResourceManager.GetString(Culture, CultureInfo.CurrentUICulture)!;
}
