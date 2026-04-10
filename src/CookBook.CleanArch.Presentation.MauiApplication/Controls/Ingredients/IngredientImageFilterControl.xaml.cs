namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Ingredients;

public partial class IngredientImageFilterControl
{
    public IngredientImageFilterControl()
    {
        InitializeComponent();
        // SyncPickerSelection(HasImage);
    }

    public static readonly BindableProperty HasImageProperty =
        BindableProperty.Create(nameof(HasImage), typeof(bool?), typeof(IngredientImageFilterControl), null, defaultBindingMode: BindingMode.TwoWay);
    public bool? HasImage
    {
        get => (bool?)GetValue(HasImageProperty);
        set => SetValue(HasImageProperty, value);
    }

}


