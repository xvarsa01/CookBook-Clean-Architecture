using System.Globalization;
using CookBook.CleanArch.Presentation.MauiApplication.Converters;
using CookBook.CleanArch.Presentation.MauiApplication.Helpers;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.Converters;

public class LocalizedNullableBoolConverterTests
{
    private readonly LocalizedNullableBoolConverter _converter = new();

    private class FakeLocalizationManager : DynamicLocalizationManager
    {
        public override string this[string key] => key switch
        {
            "HasImageFilter_Yes_Text" => "Yes",
            "HasImageFilter_No_Text" => "No",
            "HasImageFilter_Neutral_Text" => "-",
            _ => key
        };
    }

    private readonly DynamicLocalizationManager _manager = new FakeLocalizationManager();

    [Fact]
    public void Convert_True_ReturnsYes()
    {
        var result = _converter.Convert(true, typeof(string), _manager, CultureInfo.CurrentCulture);
        Assert.Equal("Yes", result);
    }

    [Fact]
    public void Convert_False_ReturnsNo()
    {
        var result = _converter.Convert(false, typeof(string), _manager, CultureInfo.CurrentCulture);
        Assert.Equal("No", result);
    }

    [Fact]
    public void Convert_Null_ReturnsDash()
    {
        var result = _converter.Convert(null, typeof(string), _manager, CultureInfo.CurrentCulture);
        Assert.Equal("-", result);
    }

    [Fact]
    public void ConvertBack_Yes_ReturnsTrue()
    {
        var result = _converter.ConvertBack("Yes", typeof(bool?), _manager, CultureInfo.CurrentCulture);
        Assert.Equal(true, result);
    }

    [Fact]
    public void ConvertBack_No_ReturnsFalse()
    {
        var result = _converter.ConvertBack("No", typeof(bool?), _manager, CultureInfo.CurrentCulture);
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBack_OtherString_ReturnsNull()
    {
        var result = _converter.ConvertBack("Maybe", typeof(bool?), _manager, CultureInfo.CurrentCulture);
        Assert.Null(result);
    }
}
