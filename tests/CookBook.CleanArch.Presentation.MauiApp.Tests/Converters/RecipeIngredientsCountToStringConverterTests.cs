using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class RecipeIngredientsCountToStringConverterTests : ConverterTestsBase
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void ConvertFrom_AnyInteger_ReturnsFormattedString(int count)
    {
        // Arrange
        var converter = CreateConverter<RecipeIngredientsCountToStringConverter>();
        var expected = string.Format(RecipeDetailViewTexts.IngredientsAmount_Label_StringFormat, count);

        // Act
        var result = converter.ConvertFrom(count, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }
}
