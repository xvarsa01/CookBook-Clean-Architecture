using System.Globalization;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class UnitToStringConverterTests : ConverterTestsBase
{
    [Theory]
    [InlineData(MeasurementUnit.Pieces, "pieces")]
    [InlineData(MeasurementUnit.Ml, "ml")]
    [InlineData(MeasurementUnit.L, "l")]
    [InlineData(MeasurementUnit.G, "g")]
    [InlineData(MeasurementUnit.Kg, "kg")]
    [InlineData(MeasurementUnit.Teaspoon, "tea spoon")]
    [InlineData(MeasurementUnit.Tablespoon, "table spoon")]
    [InlineData(MeasurementUnit.Cup, "cup")]
    [InlineData(MeasurementUnit.Pinch, "pinch")]
    [InlineData(MeasurementUnit.Dash, "dash")]
    [InlineData(MeasurementUnit.Slice, "slice")]
    [InlineData(MeasurementUnit.Can, "can")]
    [InlineData(MeasurementUnit.Bottle, "bottle")]
    [InlineData(MeasurementUnit.Pack, "pack")]
    [InlineData(MeasurementUnit.None, "none")]
    public void ConvertFrom_ValidUnit_ReturnsCorrectString(MeasurementUnit unit, string expected)
    {
        // Arrange
        var converter = CreateConverter<UnitToStringConverter>();

        // Act
        var result = converter.ConvertFrom(unit, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertFrom_UnknownValue_ReturnsNone()
    {
        // Arrange
        var converter = CreateConverter<UnitToStringConverter>();
        var unknownUnit = (MeasurementUnit)999;

        // Act
        var result = converter.ConvertFrom(unknownUnit, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("none", result);
    }
}
