namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Ingredients;

public partial class IngredientSearchControl
{
    public IngredientSearchControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty SearchTextProperty =
        BindableProperty.Create(nameof(SearchText), typeof(string), typeof(IngredientSearchControl), defaultBindingMode: BindingMode.TwoWay);
    public string? SearchText
    {
        get => (string?)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }
}

