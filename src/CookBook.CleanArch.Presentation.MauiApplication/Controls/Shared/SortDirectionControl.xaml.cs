namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Shared;

public partial class SortDirectionControl
{
    public SortDirectionControl()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty IsSortAscendingProperty =
        BindableProperty.Create(nameof(IsSortAscending), typeof(bool), typeof(SortDirectionControl));
    public bool IsSortAscending
    {
        get => (bool)GetValue(IsSortAscendingProperty);
        set => SetValue(IsSortAscendingProperty, value);
    }
}

