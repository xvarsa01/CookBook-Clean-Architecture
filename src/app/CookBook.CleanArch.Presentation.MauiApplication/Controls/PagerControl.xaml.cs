using System.Collections.ObjectModel;
using System.Windows.Input;
using CookBook.CleanArch.Presentation.MauiApplication.Models;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls;

public partial class PagerControl
{
    public PagerControl()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty PageNumbersProperty =
        BindableProperty.Create(nameof(PageNumbers), typeof(ObservableCollection<PageItem>), typeof(PagerControl));
    public ObservableCollection<PageItem> PageNumbers
    {
        get => (ObservableCollection<PageItem>)GetValue(PageNumbersProperty);
        set => SetValue(PageNumbersProperty, value);
    }
    
    public static readonly BindableProperty GoToPageCommandProperty =
        BindableProperty.Create(nameof(GoToPageCommand), typeof(ICommand), typeof(PagerControl));
    public ICommand GoToPageCommand
    {
        get => (ICommand)GetValue(GoToPageCommandProperty);
        set => SetValue(GoToPageCommandProperty, value);
    }
    
    public static readonly BindableProperty ToFirstCommandProperty =
        BindableProperty.Create(nameof(ToFirstCommand), typeof(ICommand), typeof(PagerControl));
    public ICommand ToFirstCommand
    {
        get => (ICommand)GetValue(ToFirstCommandProperty);
        set => SetValue(ToFirstCommandProperty, value);
    }
    
    public static readonly BindableProperty ToPreviousCommandProperty =
        BindableProperty.Create(nameof(ToPreviousCommand), typeof(ICommand), typeof(PagerControl));
    public ICommand ToPreviousCommand
    {
        get => (ICommand)GetValue(ToPreviousCommandProperty);
        set => SetValue(ToPreviousCommandProperty, value);
    }
        
    public static readonly BindableProperty ToNextCommandCommandProperty =
        BindableProperty.Create(nameof(ToNextCommandCommand), typeof(ICommand), typeof(PagerControl));
    public ICommand ToNextCommandCommand
    {
        get => (ICommand)GetValue(ToNextCommandCommandProperty);
        set => SetValue(ToFirstCommandProperty, value);
    }
    
    public static readonly BindableProperty ToLastCommandCommandProperty =
        BindableProperty.Create(nameof(ToLastCommandCommand), typeof(ICommand), typeof(PagerControl));
    public ICommand ToLastCommandCommand
    {
        get => (ICommand)GetValue(ToLastCommandCommandProperty);
        set => SetValue(ToFirstCommandProperty, value);
    }
}

