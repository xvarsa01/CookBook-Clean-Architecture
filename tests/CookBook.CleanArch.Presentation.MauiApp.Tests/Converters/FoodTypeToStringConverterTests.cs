using System.Globalization;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class FoodTypeToStringConverterTests : ConverterTestsBase
{
    [Theory]
    [InlineData(RecipeType.None, "none")]
    [InlineData(RecipeType.MainDish, "main dish")]
    [InlineData(RecipeType.Soup, "soup")]
    [InlineData(RecipeType.Dessert, "dessert")]
    [InlineData(RecipeType.Drink, "drink")]
    [InlineData(RecipeType.Caffe, "caffe")]
    [InlineData(RecipeType.Other, "other")]
    public void ConvertFrom_ValidFoodType_ReturnsCorrectString(RecipeType type, string expected)
    {
        // Arrange
        var converter = CreateConverter<FoodTypeToStringConverter>();

        // Act
        var result = converter.ConvertFrom(type, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertFrom_Null_ReturnsDash()
    {
        // Arrange
        var converter = CreateConverter<FoodTypeToStringConverter>();

        // Act
        var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("-", result);
    }

    [Fact]
    public void ConvertFrom_UnknownValue_ReturnsNone()
    {
        // Arrange
        var converter = CreateConverter<FoodTypeToStringConverter>();
        var unknownType = (RecipeType)999;
        converter.DefaultConvertReturnValue = FoodTypeTexts.None;

        // Act
        var result = converter.ConvertFrom(unknownType, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("none", result);
    }
}
