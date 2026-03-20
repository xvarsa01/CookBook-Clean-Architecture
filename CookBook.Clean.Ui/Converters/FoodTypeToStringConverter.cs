using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Ui.Resources.Texts;

namespace CookBook.Clean.Ui.Converters;

public class FoodTypeToStringConverter : BaseConverterOneWay<RecipeType, string>
{
    public override string ConvertFrom(RecipeType value, CultureInfo? culture)
        => FoodTypeTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? FoodTypeTexts.None;

    public override string DefaultConvertReturnValue { get; set; } = FoodTypeTexts.None;
}
