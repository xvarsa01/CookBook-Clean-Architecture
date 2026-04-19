namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Shared;

public partial class PageSizeSelectorControl
{
    public PageSizeSelectorControl()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty SelectedPageSizeProperty =
        BindableProperty.Create(nameof(SelectedPageSize), typeof(int), typeof(PageSizeSelectorControl));
    public int SelectedPageSize
    {
        get => (int)GetValue(SelectedPageSizeProperty);
        set => SetValue(SelectedPageSizeProperty, value);
    }
}

