using System.Globalization;
using CommunityToolkit.Maui.Converters;
using CookBook.Clean.Application.Models;

namespace CookBook.Clean.Ui.Converters;

public class ModelIsNewToIsVisibleConverter : BaseConverterOneWay<ModelBase, bool>
{
    public override bool ConvertFrom(ModelBase value, CultureInfo? culture)
        => value.Id == Guid.Empty;
    
    public override bool DefaultConvertReturnValue { get; set; } = true;
}
