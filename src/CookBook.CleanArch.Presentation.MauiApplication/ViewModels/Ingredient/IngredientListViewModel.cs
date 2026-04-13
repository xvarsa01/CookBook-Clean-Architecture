using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Ingredients;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Application.Ingredients.Queries;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class IngredientListViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelWithPager<IngredientFilter, IngredientsSortParameter>(messengerService), IRecipient<IngredientEditMessage>, IRecipient<IngredientDeleteMessage>
{
    [ObservableProperty]
    public partial ObservableCollection<IngredientListResponse> Ingredients { get; set; } = [];
    
    public override IngredientFilter Filter { get; set; } = new();

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        await LoadPageAsync();
    }
    
    protected override async Task LoadPageAsync()
    {
        var result = await mediator.Send(new GetIngredientListQuery(Filter, PagingOptions));
        if (!result.IsSuccess)
        {
            return;
        }

        Ingredients.Clear();
        foreach (var item in result.Value.Items)
        {
            Ingredients.Add(item);
        }
        
        TotalItemsCount = result.Value.TotalItemsCount;
        TotalPages = (int)Math.Ceiling((double)result.Value.TotalItemsCount / PagingOptions.PageSize);
        UpdatePageNumbers();
    }
    
    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync(NavigationService.IngredientCreateRouteRelative);
    }

    [RelayCommand]
    private async Task GoToDetailAsync(IngredientId id)
    {
        await navigationService.GoToAsync(NavigationService.IngredientDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(IngredientDetailViewModel.Id)] = id
            }
        );
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
