using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class ViewModelWithPager<TFilter, TSortParameter>(IMessengerService messengerService)
    : ViewModelBase(messengerService)
    where TFilter : IFilter<TSortParameter>, new()
{
    [ObservableProperty]
    public partial ObservableCollection<PageItem> PageNumbers { get; set; } = [];

    protected int CurrentPage => PagingOptions.PageIndex + 1;
    
    public virtual TFilter Filter { get; set; } = new();

    public PagingOptions PagingOptions { get; set; } = new ()
    {
        PageIndex = 0,
        PageSize = 5
    };

    protected virtual Task LoadPageAsync()
        => Task.CompletedTask;
    
    [ObservableProperty]
    public partial int TotalItemsCount { get; set; }

    [ObservableProperty]
    public partial int TotalPages { get; set; }
    
    [RelayCommand]
    private async Task ApplyFilterAsync()
    {
        PagingOptions.PageIndex = 0;
        await LoadPageAsync();
    }
    
    [RelayCommand]
    private async Task ToFirstPageAsync()
    {
        PagingOptions.PageIndex = 0;
        await LoadPageAsync();
    }
    
    [RelayCommand]
    private async Task ToPreviousPageAsync()
    {
        if (PagingOptions.PageIndex > 0)
        {
            PagingOptions.PageIndex--;
        }

        await LoadPageAsync();
    }
    
    [RelayCommand]
    private async Task ToNextPageAsync()
    {
        if (PagingOptions.PageIndex < TotalPages - 1)
        {
            PagingOptions.PageIndex++;
        }

        await LoadPageAsync();
    }
    
    [RelayCommand]
    private async Task ToLastPageAsync()
    {
        if (TotalPages <= 0)
        {
            return;
        }

        PagingOptions.PageIndex = TotalPages - 1;
        await LoadPageAsync();
    }

    [RelayCommand]
    private async Task GoToPageAsync(int pageNumber)
    {
        if (pageNumber < 1 || pageNumber > TotalPages)
        {
            return;
        }

        PagingOptions.PageIndex = pageNumber - 1;
        await LoadPageAsync();
    }

    protected void UpdatePageNumbers()
    {
        PageNumbers.Clear();

        if (TotalPages <= 0)
            return;

        foreach (var i in Enumerable.Range(1, TotalPages))
        {
            var model = new PageItem
            {
                Number = i,
                IsCurrent = i == CurrentPage
            };
            PageNumbers.Add(model);
        }
    }
}
