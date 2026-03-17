using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.Clean.App.Resources.Texts;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.App.Converters;

public class UnitToStringConverter : BaseConverterOneWay<MeasurementUnit, string>
{
    public override string ConvertFrom(MeasurementUnit value, CultureInfo? culture)
        => UnitTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? UnitTexts.None;
    public override string DefaultConvertReturnValue { get; set; } = UnitTexts.None;
}
