using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.Clean.App.Resources.Texts;

namespace CookBook.Clean.App.Converters;

public class RecipeIngredientsCountToStringConverter : BaseConverterOneWay<int, string>
{
    public override string ConvertFrom(int value, CultureInfo? culture)
        => string.Format(RecipeDetailViewTexts.IngredientsAmount_Label_StringFormat, value);

    public override string DefaultConvertReturnValue { get; set; } = string.Empty;
}
