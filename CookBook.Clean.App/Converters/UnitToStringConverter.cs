using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.Clean.App.Resources.Texts;
using MediatR;

namespace CookBook.Clean.App.Converters;

public class UnitToStringConverter : BaseConverterOneWay<Unit, string>
{
    public override string ConvertFrom(Unit value, CultureInfo? culture)
        => UnitTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? UnitTexts.None;
    public override string DefaultConvertReturnValue { get; set; } = UnitTexts.None;
}
