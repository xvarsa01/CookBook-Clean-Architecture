using CookBook.CleanArch.Domain.Recipe.Enums;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Recipe;

public partial class RecipeTypePickerControl
{
    public RecipeTypePickerControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty SelectedRecipeTypeProperty =
        BindableProperty.Create(nameof(SelectedRecipeType), typeof(RecipeType?), typeof(RecipeTypePickerControl), defaultBindingMode: BindingMode.TwoWay);

    public RecipeType? SelectedRecipeType
    {
        get => (RecipeType?)GetValue(SelectedRecipeTypeProperty);
        set => SetValue(SelectedRecipeTypeProperty, value);
    }

}



