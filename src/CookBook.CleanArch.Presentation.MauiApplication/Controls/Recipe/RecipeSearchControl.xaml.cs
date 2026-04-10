namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Recipe;

public partial class RecipeSearchControl
{
    public RecipeSearchControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty SearchTextProperty =
        BindableProperty.Create(nameof(SearchText), typeof(string), typeof(RecipeSearchControl), defaultBindingMode: BindingMode.TwoWay);
    public string? SearchText
    {
        get => (string?)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }
}

