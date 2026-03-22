using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.Ui.Converters;

public class FoodTypeToColorConverter : BaseConverterOneWay<RecipeType, Color>
{
    public override Color ConvertFrom(RecipeType value, CultureInfo? culture)
    {
        return ((Microsoft.Maui.Controls.Application.Current?.Resources.TryGetValue($"{value}FoodTypeColor", out var resource) is true)
                && (resource is Color color))
                ? color
                : DefaultConvertReturnValue;
    }

    public override Color DefaultConvertReturnValue { get; set; } = Colors.Transparent;
}
