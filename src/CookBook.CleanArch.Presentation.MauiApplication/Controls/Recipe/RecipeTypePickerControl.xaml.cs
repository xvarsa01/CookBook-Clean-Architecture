using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Helpers;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Recipe;

public partial class RecipeTypePickerControl
{
    public RecipeTypePickerControl()
    {
        InitializeComponent();
        
        var manager = (DynamicLocalizationManager)Microsoft.Maui.Controls.Application.Current!.Resources["RecipeControlsLocalizationManager"];
        manager.PropertyChanged += (_, __) => RefreshOptions();
        
    }

    public static readonly BindableProperty SelectedRecipeTypeProperty =
        BindableProperty.Create(nameof(SelectedRecipeType), typeof(RecipeType?), typeof(RecipeTypePickerControl), defaultBindingMode: BindingMode.TwoWay);

    public RecipeType? SelectedRecipeType
    {
        get => (RecipeType?)GetValue(SelectedRecipeTypeProperty);
        set => SetValue(SelectedRecipeTypeProperty, value);
    }

    public List<RecipeType?> RecipeTypes { get; private set; } =
        new List<RecipeType?> { null }
            .Concat(Enum.GetValues<RecipeType>().Cast<RecipeType?>())
            .ToList();
    
    private void RefreshOptions()
    {
        RecipeTypes = new List<RecipeType?> { null }
            .Concat(Enum.GetValues<RecipeType>().Cast<RecipeType?>())
            .ToList();
        OnPropertyChanged(nameof(RecipeTypes));
    }
}



