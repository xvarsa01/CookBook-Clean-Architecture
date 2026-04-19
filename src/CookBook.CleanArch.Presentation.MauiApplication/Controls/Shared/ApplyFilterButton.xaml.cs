using System.Windows.Input;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Shared;

public partial class ApplyFilterButton
{
    public ApplyFilterButton()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty ApplyCommandProperty =
        BindableProperty.Create(nameof(ApplyCommand), typeof(ICommand), typeof(PagerControl));
    public ICommand ApplyCommand
    {
        get => (ICommand)GetValue(ApplyCommandProperty);
        set => SetValue(ApplyCommandProperty, value);
    }
}

