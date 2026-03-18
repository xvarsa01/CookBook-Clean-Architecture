using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.App.Messages;
using CookBook.Clean.App.Services;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.RecipeRoot.Delete;
using CookBook.Clean.UseCases.RecipeRoot.Get;
using MediatR;

namespace CookBook.Clean.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeDetailViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<RecipeEditMessage>, IRecipient<RecipeIngredientAddMessage>,
        IRecipient<RecipeIngredientDeleteMessage>
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public partial RecipeDetailModel? Recipe { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await _mediator.Send(new GetRecipeQuery(Id)));
        if (result.Success && result.Value is not null)
        {
            Recipe = result.Value;
            // converted with color not called
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Recipe is not null)
        {
            var result = (await _mediator.Send(new DeleteRecipeUseCase(Recipe.Id)));

            MessengerService.Send(new RecipeDeleteMessage());

            navigationService.SendBackButtonPressed();
        }
    }


    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if (Recipe is not null)
        {
            await navigationService.GoToAsync(NavigationService.RecipeEditRouteRelative,
                new Dictionary<string, object?>
                {
                    [nameof(RecipeEditViewModel.Id)] = Recipe.Id
                }
            );
        }
    }

    public void Receive(RecipeEditMessage message)
    {
        if (message.RecipeId == Recipe?.Id)
        {
            ForceDataRefreshOnNextAppearing();
        }
    }

    public void Receive(RecipeIngredientAddMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(RecipeIngredientDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}
