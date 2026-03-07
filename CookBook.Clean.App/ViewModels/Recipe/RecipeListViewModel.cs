using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.App.Messages;
using CookBook.Clean.App.Services;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.Recipe;
using CookBook.Clean.UseCases.Recipe.GetList;
using MediatR;

namespace CookBook.Clean.App.ViewModels;

public partial class RecipeListViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<RecipeEditMessage>, IRecipient<RecipeDeleteMessage>
{
    [ObservableProperty]
    public partial IEnumerable<RecipeListModel> Recipes { get; set; } = [];

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await _mediator.Send(new GetListRecipeUseCase())).Value?.Recipes;
        if (result is not null)
        {
            Recipes = result;
        }
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
        => await navigationService.GoToAsync(NavigationService.RecipeDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(RecipeDetailViewModel.Id)] = id
            }
        );

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync(NavigationService.RecipeEditRouteRelative);
    }

    public void Receive(RecipeEditMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(RecipeDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}
