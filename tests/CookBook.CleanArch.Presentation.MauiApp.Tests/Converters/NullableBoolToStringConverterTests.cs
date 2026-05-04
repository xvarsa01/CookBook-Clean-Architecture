using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class NullableBoolToStringConverterTests
{
    private readonly NullableBoolToStringConverter _converter = new();

    [Fact]
    public void Convert_True_ReturnsYes()
    {
        // Act
        var result = _converter.Convert(true, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal("Yes", result);
    }

    [Fact]
    public void Convert_False_ReturnsNo()
    {
        // Act
        var result = _converter.Convert(false, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal("No", result);
    }

    [Fact]
    public void Convert_Null_ReturnsDash()
    {
        // Act
        var result = _converter.Convert(null, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal("-", result);
    }

    [Fact]
    public void ConvertBack_Yes_ReturnsTrue()
    {
        // Act
        var result = _converter.ConvertBack("Yes", typeof(bool?), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void ConvertBack_No_ReturnsFalse()
    {
        // Act
        var result = _converter.ConvertBack("No", typeof(bool?), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBack_OtherString_ReturnsNull()
    {
        // Act
        var result = _converter.ConvertBack("Maybe", typeof(bool?), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Null(result);
    }
}

