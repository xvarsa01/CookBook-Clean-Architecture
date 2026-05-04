namespace CookBook.CleanArch.Presentation.MauiApplication.Helpers;

[ContentProperty(nameof(Text))]
[AcceptEmptyServiceProvider]
public class DynamicTranslateExtension : IMarkupExtension<BindingBase>
{
    public string Text { get; set; } = string.Empty;

    public DynamicLocalizationManager? LocalizationManager { get; set; }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        return new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"[{Text}]",
            Source = LocalizationManager
        };
    }
}
