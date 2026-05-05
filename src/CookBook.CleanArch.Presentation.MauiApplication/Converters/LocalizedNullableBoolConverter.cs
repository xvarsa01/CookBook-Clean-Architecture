using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Helpers;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class LocalizedNullableBoolConverter : IValueConverter
{
    private const string YesKey = "HasImageFilter_Yes_Text";
    private const string NoKey = "HasImageFilter_No_Text";
    private const string NeutralKey = "HasImageFilter_Neutral_Text";

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var manager = parameter as DynamicLocalizationManager;
        if (manager == null)
            return value?.ToString() ?? "-";

        return value switch
        {
            true => manager[YesKey],
            false => manager[NoKey],
            _ => manager[NeutralKey]
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var manager = parameter as DynamicLocalizationManager;
        if (manager == null || value is not string str)
            return null;

        if (str == manager[YesKey]) return true;
        if (str == manager[NoKey]) return false;
        return null;
    }
}
