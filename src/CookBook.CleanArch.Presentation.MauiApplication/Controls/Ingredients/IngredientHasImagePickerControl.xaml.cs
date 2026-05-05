using CookBook.CleanArch.Presentation.MauiApplication.Helpers;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Ingredients;

public partial class IngredientHasImagePickerControl
{
    public IngredientHasImagePickerControl()
    {
        InitializeComponent();
        
        var manager = (DynamicLocalizationManager)Microsoft.Maui.Controls.Application.Current!.Resources["IngredientsControlsLocalizationManager"];
        manager.PropertyChanged += (_, __) => RefreshOptions();
    }

    public static readonly BindableProperty HasImageProperty =
        BindableProperty.Create(nameof(HasImage), typeof(bool?), typeof(IngredientHasImagePickerControl), null, defaultBindingMode: BindingMode.TwoWay);
    public bool? HasImage
    {
        get => (bool?)GetValue(HasImageProperty);
        set => SetValue(HasImageProperty, value);
    }
    
    public IList<bool?> Options { get; private set; } = new List<bool?>
    {
        null, true, false
    };
    
    private void RefreshOptions()
    {
        Options = new List<bool?> { null, true, false };
        OnPropertyChanged(nameof(Options));
    }

}
