using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;
using FluentValidation.Results;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class ValidationResultToHasNoErrorConverterTests
{
    private readonly ValidationResultToHasNoErrorConverter _converter = new();

    [Fact]
    public void Convert_ValidResult_ReturnsTrue()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var result = _converter.Convert(validationResult, typeof(bool), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.True((bool)result!);
    }

    [Fact]
    public void Convert_InvalidResultWithMatchingError_ReturnsFalse()
    {
        // Arrange
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Property", "Error") });

        // Act
        var result = _converter.Convert(validationResult, typeof(bool), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.False((bool)result!);
    }

    [Fact]
    public void Convert_InvalidResultWithoutMatchingError_ReturnsTrue()
    {
        // Arrange
        var validationResult = new ValidationResult(new[] { new ValidationFailure("OtherProperty", "Error") });

        // Act
        var result = _converter.Convert(validationResult, typeof(bool), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.True((bool)result!);
    }

    [Fact]
    public void Convert_InvalidResultWithNullParameter_ReturnsFalse()
    {
        // Arrange
        var validationResult = new ValidationResult(new[] { new ValidationFailure("AnyProperty", "Error") });

        // Act
        var result = _converter.Convert(validationResult, typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.False((bool)result!);
    }

    [Fact]
    public void Convert_NullValue_ReturnsNull()
    {
        // Act
        var result = _converter.Convert(null, typeof(bool), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Assert
        Assert.Throws<NotImplementedException>(() => _converter.ConvertBack(null, typeof(object), null, CultureInfo.CurrentCulture));
    }
}

