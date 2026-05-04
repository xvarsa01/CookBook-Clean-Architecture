using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;
using FluentValidation.Results;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class ValidationResultToErrorMessagesConverterTests
{
    private readonly ValidationResultToErrorMessagesConverter _converter = new();

    [Fact]
    public void Convert_ValidResult_ReturnsNull()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var result = _converter.Convert(validationResult, typeof(string), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Convert_InvalidResultWithMatchingError_ReturnsErrorMessage()
    {
        // Arrange
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Property", "Error message")
        });

        // Act
        var result = _converter.Convert(validationResult, typeof(string), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal("Error message", result);
    }

    [Fact]
    public void Convert_InvalidResultWithMultipleMatchingErrors_ReturnsCombinedErrorMessages()
    {
        // Arrange
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Property", "Error 1"),
            new ValidationFailure("Property", "Error 2")
        });
        var expected = $"Error 1{Environment.NewLine}Error 2";

        // Act
        var result = _converter.Convert(validationResult, typeof(string), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_InvalidResultWithoutMatchingError_ReturnsNull()
    {
        // Arrange
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("OtherProperty", "Error message")
        });

        // Act
        var result = _converter.Convert(validationResult, typeof(string), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Convert_NullValue_ReturnsNull()
    {
        // Act
        var result = _converter.Convert(null, typeof(string), "Property", CultureInfo.CurrentCulture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Convert_NullParameter_ReturnsNull()
    {
        // Arrange
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Property", "Error message")
        });

        // Act
        var result = _converter.Convert(validationResult, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Null(result);
    }
}

