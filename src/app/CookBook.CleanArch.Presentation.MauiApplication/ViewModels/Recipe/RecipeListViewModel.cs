using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

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

        var filter = new RecipeFilter();
        var result = (await _mediator.Send(new GetRecipeListQuery(filter))).Value;
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
