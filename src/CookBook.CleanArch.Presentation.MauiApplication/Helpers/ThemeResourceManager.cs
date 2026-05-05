using Microsoft.Maui.Controls;
using System.Collections.Generic;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Styles;

namespace CookBook.CleanArch.Presentation.MauiApplication.Helpers;

public static class ThemeResourceManager
{
    public static void ApplyThemeResources(AppTheme theme)
    {
        var app = global::Microsoft.Maui.Controls.Application.Current;
        if (app?.Resources is null)
        {
            return;
        }

        var dictionaries = app.Resources.MergedDictionaries;
        RemoveColorsDictionary(dictionaries);

        ResourceDictionary dictionary = theme == AppTheme.Light
            ? new ColorsLight()
            : new ColorsDark();
        dictionaries.Add(dictionary);
    }

    private static void RemoveColorsDictionary(ICollection<ResourceDictionary> dictionaries)
    {
        var toRemove = new List<ResourceDictionary>();
        foreach (var dictionary in dictionaries)
        {
            if (dictionary is ColorsLight or ColorsDark)
            {
                toRemove.Add(dictionary);
            }
        }

        foreach (var dictionary in toRemove)
        {
            dictionaries.Remove(dictionary);
        }
    }
}
