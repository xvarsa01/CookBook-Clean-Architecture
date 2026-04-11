using System.Globalization;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class NullableBoolToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            true => "Yes",
            false => "No",
            _ => "-"
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            "Yes" => true,
            "No" => false,
            _ => null
        };
    }
}
