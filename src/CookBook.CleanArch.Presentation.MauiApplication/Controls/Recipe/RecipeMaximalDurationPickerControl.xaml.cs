using CookBook.CleanArch.Domain.Recipes.Enums;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Recipe;

public partial class RecipeMaximalDurationPickerControl
{
    public RecipeMaximalDurationPickerControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty UsePickerProperty =
        BindableProperty.Create(nameof(UsePicker), typeof(bool?), typeof(RecipeMinimalDurationPickerControl), null, defaultBindingMode: BindingMode.TwoWay);
    public bool? UsePicker
    {
        get => (bool?)GetValue(UsePickerProperty);
        set => SetValue(UsePickerProperty, value);
    }
    
    public static readonly BindableProperty SelectedValueProperty =
        BindableProperty.Create(nameof(SelectedValue), typeof(TimeSpan?), typeof(RecipeMinimalDurationPickerControl), null, defaultBindingMode: BindingMode.TwoWay);
    public TimeSpan? SelectedValue
    {
        get => (TimeSpan?)GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

}



