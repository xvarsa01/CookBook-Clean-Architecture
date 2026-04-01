using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class FoodTypeToStringConverter : BaseConverterOneWay<RecipeType?, string>
{
    public override string ConvertFrom(RecipeType? value, CultureInfo? culture)
        => value is null
            ? FoodTypeTexts.None
            : FoodTypeTexts.ResourceManager.GetString(value.Value.ToString(), culture)
              ?? FoodTypeTexts.None;

    public override string DefaultConvertReturnValue { get; set; } = FoodTypeTexts.None;
}
