using System.Globalization;
using FluentValidation.Results;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class ValidationResultToHasErrorConverter : IValueConverter
{
    /// <summary>
    /// Converts a ValidationResult to a boolean indicating if the property has an error
    /// </summary>
    /// <param name="value">The ValidationResult</param>
    /// <param name="targetType">The target type</param>
    /// <param name="parameter">The property name to check if there are some errors</param>
    /// <param name="culture"></param>
    /// <returns>True if there is at least one error for the specified property, else false</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ValidationResult validationResult)
        {
            return null;
        }

        if (validationResult.IsValid)
        {
            return false;
        }

        // If no specific property is provided → check all errors
        if (parameter is null)
        {
            return validationResult.Errors.Any();
        }

        var property = parameter as string;

        return validationResult.Errors.Any(x => x.PropertyName == property);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("ConvertBack not implemented for the converter.");
    }
}
