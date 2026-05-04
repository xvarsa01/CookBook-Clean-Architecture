using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class InverseBooleanConverterTests
{
    private readonly InverseBooleanConverter _converter = new();

    [Fact]
    public void Convert_True_ReturnsFalse()
    {
        // Act
        var result = _converter.Convert(true, typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.IsType<bool>(result);
        Assert.False((bool)result);
    }

    [Fact]
    public void Convert_False_ReturnsTrue()
    {
        // Act
        var result = _converter.Convert(false, typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.IsType<bool>(result);
        Assert.True((bool)result);
    }

    [Fact]
    public void Convert_Null_ReturnsFalse()
    {
        // Act
        var result = _converter.Convert(null, typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.IsType<bool>(result);
        Assert.False((bool)result);
    }

    [Fact]
    public void ConvertBack_BehavesSameAsConvert()
    {
        // Act
        var resultTrue = _converter.ConvertBack(true, typeof(bool), null, CultureInfo.CurrentCulture);
        var resultFalse = _converter.ConvertBack(false, typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.False((bool)resultTrue);
        Assert.True((bool)resultFalse);
    }
}

