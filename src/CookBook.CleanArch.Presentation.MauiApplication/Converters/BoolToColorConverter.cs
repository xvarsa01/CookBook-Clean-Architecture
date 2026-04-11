using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class BoolToColorConverter : BaseConverterOneWay<bool, Color>
{
    public override Color ConvertFrom(bool value, CultureInfo? culture)
    {
        var resourceKey = value ? "PrimaryColor" : "UnselectedColor";
        return (Microsoft.Maui.Controls.Application.Current?.Resources.TryGetValue(resourceKey, out var resource) is true
                && resource is Color color)
            ? color
            : DefaultConvertReturnValue;
    }

    public override Color DefaultConvertReturnValue { get; set; } = Colors.Transparent;
}
