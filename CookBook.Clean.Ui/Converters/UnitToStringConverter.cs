using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Ui.Resources.Texts;
using MediatR;

namespace CookBook.Clean.Ui.Converters;

public class UnitToStringConverter : BaseConverterOneWay<MeasurementUnit, string>
{
    public override string ConvertFrom(MeasurementUnit value, CultureInfo? culture)
        => UnitTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? UnitTexts.None;
    public override string DefaultConvertReturnValue { get; set; } = UnitTexts.None;
}
