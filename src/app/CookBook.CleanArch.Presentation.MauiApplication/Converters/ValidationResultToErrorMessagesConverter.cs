using System.Globalization;
using FluentValidation.Results;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class ValidationResultToErrorMessagesConverter : IValueConverter
{
    /// <summary>
    /// Converts a ValidationResult to a string containing all error messages for a specific property
    /// </summary>
    /// <param name="value">The ValidationResult</param>
    /// <param name="targetType">The targeted type</param>
    /// <param name="parameter">The property name to check if there are some errors</param>
    /// <param name="culture"></param>
    /// <returns>Error messages for the specified property</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // If the value is not a ValidationResult or the ValidationResult is valid, return null
        if (value is not ValidationResult validationResult || validationResult.IsValid)
        {
            return null;
        }

        if (parameter == null)
        {
            return null;
        }

        var property = parameter as string;

        // Get all error messages for the specified property
        IEnumerable<string> errorMessages = validationResult.Errors
            .Where(x => x.PropertyName == property)
            .Select(x => x.ErrorMessage);

        // Return all error messages as a single string
        return string.Join(Environment.NewLine, errorMessages);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("ConvertBack not implemented for the converter.");
    }
}
