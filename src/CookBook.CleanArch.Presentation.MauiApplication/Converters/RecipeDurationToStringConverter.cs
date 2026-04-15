using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class RecipeDurationToStringConverter : BaseConverterOneWay<TimeSpan, string>
{
    public override string ConvertFrom(TimeSpan value, CultureInfo? culture)
    {
        // Duration values are user-facing; round to whole minutes and never show seconds.
        var roundedMinutes = Math.Max(1, (int)Math.Round(value.TotalMinutes, MidpointRounding.AwayFromZero));

        if (roundedMinutes < 60)
        {
            return string.Format(GetResource("Duration_MinutesOnly_StringFormat", culture), roundedMinutes);
        }

        var hours = roundedMinutes / 60;
        var minutes = roundedMinutes % 60;

        return string.Format(GetResource("Duration_HoursAndMinutes_StringFormat", culture), hours, minutes);
    }

    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    private static string GetResource(string key, CultureInfo? culture)
    {
        return RecipeDetailViewTexts.ResourceManager.GetString(key, culture)
               ?? RecipeDetailViewTexts.ResourceManager.GetString(key, RecipeDetailViewTexts.Culture)
               ?? string.Empty;
    }
}

