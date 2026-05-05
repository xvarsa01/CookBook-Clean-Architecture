using System.Globalization;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class IsCountNotZeroConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int count)
            return true;

        return count != 0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
