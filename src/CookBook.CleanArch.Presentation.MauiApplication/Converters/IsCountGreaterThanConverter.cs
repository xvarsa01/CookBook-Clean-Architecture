using System.Globalization;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class IsCountGreaterThanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int count)
            return false;

        if (parameter is null || !int.TryParse(parameter.ToString(), out var threshold))
            return false;

        return count > threshold;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
