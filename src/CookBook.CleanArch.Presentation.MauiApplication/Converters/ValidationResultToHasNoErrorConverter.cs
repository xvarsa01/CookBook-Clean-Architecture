using System.Globalization;
using FluentValidation.Results;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class ValidationResultToHasNoErrorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ValidationResult validationResult)
        {
            return null;
        }

        // No parameter → check entire object
        if (parameter is null)
        {
            return validationResult.IsValid;
        }

        var property = parameter as string;

        // No errors for this property
        return validationResult.Errors.All(x => x.PropertyName != property);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
