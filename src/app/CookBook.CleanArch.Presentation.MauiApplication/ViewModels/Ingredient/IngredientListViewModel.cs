using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
    : ViewModelWithPager<IngredientFilter, IngredientsSortParameter>(messengerService), IRecipient<IngredientEditMessage>, IRecipient<IngredientDeleteMessage>
{
    [ObservableProperty]
    public partial ObservableCollection<IngredientListModel> Ingredients { get; set; } = [];
    
    public override IngredientFilter Filter { get; set; } = new();

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        await LoadPageAsync();
    }
    
    protected override async Task LoadPageAsync()
    {
        var result = await _mediator.Send(new GetIngredientListQuery(Filter, PagingOptions));
        if (!result.IsSuccess)
        {
            return;
        }

        Ingredients.Clear();
        foreach (var item in result.Value.Items)
        {
            Ingredients.Add(IngredientListModel.MapFromResponse(item));
        }
        
        TotalItemsCount = result.Value.TotalItemsCount;
        TotalPages = (int)Math.Ceiling((double)result.Value.TotalItemsCount / PagingOptions.PageSize);
        UpdatePageNumbers();
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

    public void Receive(IngredientEditMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(IngredientDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}
