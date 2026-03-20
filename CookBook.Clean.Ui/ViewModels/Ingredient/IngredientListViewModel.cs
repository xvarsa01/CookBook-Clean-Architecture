using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Ui.Messages;
using CookBook.Clean.Ui.Services;
using CookBook.Clean.Ui.Services.Interfaces;
using MediatR;

namespace CookBook.Clean.Ui.ViewModels;

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
        
        var result = (await _mediator.Send(new GetListIngredientQuery(filter))).Value;
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
