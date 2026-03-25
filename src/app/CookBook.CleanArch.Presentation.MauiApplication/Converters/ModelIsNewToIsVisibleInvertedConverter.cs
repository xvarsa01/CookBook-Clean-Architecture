using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.CleanArch.Application.Models;

namespace CookBook.CleanArch.Presentation.MauiApplication.Converters;

public class ModelIsNewToIsVisibleInvertedConverter : BaseConverterOneWay<IModel, bool>
{
    public override bool ConvertFrom(IModel value, CultureInfo? culture)
        => value.Id != Guid.Empty;
    
    public override bool DefaultConvertReturnValue { get; set; } = true;
}
