using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class IngredientListViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<IngredientEditMessage>, IRecipient<IngredientDeleteMessage>
{
    [ObservableProperty]
    public partial IEnumerable<IngredientListModel> Ingredients { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<PageItem> PageNumbers { get; set; } = [];

    private int CurrentPage => PagingOptions.PageIndex + 1;
    
    public IngredientFilter Filter { get; set; } = new ();
    public PagingOptions PagingOptions { get; set; } = new ()
    {
        PageIndex = 0,
        PageSize = 5
    };

    [ObservableProperty]
    public partial int TotalItemsCount { get; set; }

    [ObservableProperty]
    public partial int TotalPages { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        await LoadPageAsync();
    }

    [RelayCommand]
    private async Task ApplyFilterAsync()
    {
        PagingOptions.PageIndex = 0;
        await LoadPageAsync();
    }
    
    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync(NavigationService.IngredientEditRouteRelative);
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync(NavigationService.IngredientDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(IngredientDetailViewModel.Id)] = new IngredientId(id)
            }
        );
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

    private async Task LoadPageAsync()
    {
        Result<PagedResult<IngredientListResponse>> result = await _mediator.Send(new GetIngredientListQuery(Filter, PagingOptions));
        if (!result.IsSuccess)
        {
            return;
        }

        Ingredients = new ObservableCollection<IngredientListModel>(result.Value.Items.Select(IngredientListModel.MapFromResponse));
        TotalItemsCount = result.Value.TotalItemsCount;
        TotalPages = (int)Math.Ceiling((double)result.Value.TotalItemsCount / PagingOptions.PageSize);
        UpdatePageNumbers();
    }

    private void UpdatePageNumbers()
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

    public void Receive(IngredientEditMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(IngredientDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}
