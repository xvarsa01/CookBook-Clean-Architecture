using System.Globalization;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class NullToFalseConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is not null;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

