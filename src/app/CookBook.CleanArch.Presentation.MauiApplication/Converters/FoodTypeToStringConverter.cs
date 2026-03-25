using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class FoodTypeToStringConverter : BaseConverterOneWay<RecipeType, string>
{
    public override string ConvertFrom(RecipeType value, CultureInfo? culture)
        => FoodTypeTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? FoodTypeTexts.None;

    public override string DefaultConvertReturnValue { get; set; } = FoodTypeTexts.None;
}
