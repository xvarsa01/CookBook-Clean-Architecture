using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.CleanArch.Domain.RecipeRoot.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class UnitToStringConverter : BaseConverterOneWay<MeasurementUnit, string>
{
    public override string ConvertFrom(MeasurementUnit value, CultureInfo? culture)
        => UnitTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? UnitTexts.None;
    public override string DefaultConvertReturnValue { get; set; } = UnitTexts.None;
}
