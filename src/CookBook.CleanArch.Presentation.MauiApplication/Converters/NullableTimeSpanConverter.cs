using System.Globalization;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class NullableTimeSpanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value ?? TimeSpan.Zero;
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan ts)
        {
            return ts == TimeSpan.Zero ? null : ts;
        }
        return null;
    }

}
