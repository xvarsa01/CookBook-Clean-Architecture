using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class IsCountZeroConverterTests
{
    private readonly IsCountZeroConverter _converter = new();

    [Fact]
    public void Convert_ZeroIngredients_ReturnsTrue()
    {
        // Act
        var result = _converter.Convert(0, typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.IsType<bool>(result);
        Assert.True((bool)result);
    }

    [Fact]
    public void Convert_OneIngredient_ReturnsFalse()
    {
        // Act
        var result = _converter.Convert(1, typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.IsType<bool>(result);
        Assert.False((bool)result);
    }

    [Fact]
    public void Convert_MultipleIngredients_ReturnsFalse()
    {
        // Act
        var result = _converter.Convert(5, typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.IsType<bool>(result);
        Assert.False((bool)result);
    }

    [Fact]
    public void Convert_InvalidType_ReturnsFalse()
    {
        // Act
        var result = _converter.Convert("not an int", typeof(bool), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.IsType<bool>(result);
        Assert.False((bool)result);
    }
}

