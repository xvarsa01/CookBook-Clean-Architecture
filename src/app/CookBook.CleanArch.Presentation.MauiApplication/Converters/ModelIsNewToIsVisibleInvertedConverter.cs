using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.CleanArch.Application.Abstraction;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class ModelIsNewToIsVisibleInvertedConverter : BaseConverterOneWay<IModel, bool>
{
    public override bool ConvertFrom(IModel value, CultureInfo? culture)
        => value.Id.Value != Guid.Empty;
    
    public override bool DefaultConvertReturnValue { get; set; } = true;
}
