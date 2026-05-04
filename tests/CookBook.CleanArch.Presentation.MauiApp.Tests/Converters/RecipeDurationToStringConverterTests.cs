using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class RecipeDurationToStringConverterTests : ConverterTestsBase
{
    [Fact]
    public void ConvertFrom_LessThanOneHour_ReturnsMinutes()
    {
        // Arrange
        var converter = CreateConverter<RecipeDurationToStringConverter>();
        var duration = TimeSpan.FromMinutes(45);
        var expected = string.Format(RecipeDetailViewTexts.Duration_MinutesOnly_StringFormat, 45);

        // Act
        var result = converter.ConvertFrom(duration, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertFrom_ExactlyOneHour_ReturnsHoursAndMinutes()
    {
        // Arrange
        var converter = CreateConverter<RecipeDurationToStringConverter>();
        var duration = TimeSpan.FromMinutes(60);
        var expected = string.Format(RecipeDetailViewTexts.Duration_HoursAndMinutes_StringFormat, 1, 0);

        // Act
        var result = converter.ConvertFrom(duration, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertFrom_MoreThanOneHour_ReturnsHoursAndMinutes()
    {
        // Arrange
        var converter = CreateConverter<RecipeDurationToStringConverter>();
        var duration = TimeSpan.FromMinutes(95);
        var expected = string.Format(RecipeDetailViewTexts.Duration_HoursAndMinutes_StringFormat, 1, 35);

        // Act
        var result = converter.ConvertFrom(duration, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertFrom_RoundsUpToNearestMinute()
    {
        // Arrange
        var converter = CreateConverter<RecipeDurationToStringConverter>();
        var duration = TimeSpan.FromSeconds(89); // 1 minute 29 seconds
        var expected = string.Format(RecipeDetailViewTexts.Duration_MinutesOnly_StringFormat, 1);

        // Act
        var result = converter.ConvertFrom(duration, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void ConvertFrom_RoundsUpToNearestMinute2()
    {
        // Arrange
        var converter = CreateConverter<RecipeDurationToStringConverter>();
        var duration = TimeSpan.FromSeconds(91); // 1 minute 31 seconds
        var expected = string.Format(RecipeDetailViewTexts.Duration_MinutesOnly_StringFormat, 2);

        // Act
        var result = converter.ConvertFrom(duration, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }
}
