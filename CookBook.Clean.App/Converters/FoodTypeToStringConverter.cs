using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.Clean.App.Resources.Texts;
using CookBook.Clean.Core.Recipe;

namespace CookBook.Clean.App.Converters;

public class FoodTypeToStringConverter : BaseConverterOneWay<RecipeType, string>
{
    public override string ConvertFrom(RecipeType value, CultureInfo? culture)
        => FoodTypeTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? FoodTypeTexts.None;

    public override string DefaultConvertReturnValue { get; set; } = FoodTypeTexts.None;
}
