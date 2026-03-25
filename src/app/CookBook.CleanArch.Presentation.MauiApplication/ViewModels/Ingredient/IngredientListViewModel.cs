using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
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

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var filter = new IngredientFilter();
        
        var result = (await _mediator.Send(new GetIngredientListQuery(filter))).Value;
        if (result is not null)
        {
            Ingredients = result;
        }
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
