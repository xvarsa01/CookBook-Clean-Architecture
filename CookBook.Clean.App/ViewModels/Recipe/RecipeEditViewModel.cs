using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.App.Messages;
using CookBook.Clean.App.Services;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.RecipeRoot.Create;
using CookBook.Clean.Application.RecipeRoot.Get;
using CookBook.Clean.Application.RecipeRoot.Update;
using MediatR;

namespace CookBook.Clean.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeEditViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<RecipeIngredientEditMessage>, IRecipient<RecipeIngredientAddMessage>,
        IRecipient<RecipeIngredientDeleteMessage>
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public partial RecipeDetailModel Recipe { get; set; } = RecipeDetailModel.Empty;

    public List<RecipeType> FoodTypes { get; set; } = [.. Enum.GetValues<RecipeType>()];

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await _mediator.Send(new GetRecipeQuery(Id)));
        if (result.Success && result.Value is not null)
        {
            Recipe = result.Value;
        }
    }

    [RelayCommand]
    private async Task GoToRecipeIngredientEditAsync()
    {
        await navigationService.GoToAsync(NavigationService.RecipeIngredientsEditRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(RecipeIngredientsEditViewModel.Id)] = Recipe.Id
                });
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Recipe.Id == Guid.Empty)
        {
            await _mediator.Send(new CreateRecipeUseCase(Recipe.Name, Recipe.Description, Recipe.ImageUrl, Recipe.Duration, Recipe.Type));
        }
        else
        {
            await _mediator.Send(new UpdateRecipeUseCase(Recipe.Id, Recipe.Name, Recipe.Description, Recipe.ImageUrl, Recipe.Duration, Recipe.Type));
        }

        MessengerService.Send(new RecipeEditMessage { RecipeId = Recipe.Id});

        navigationService.SendBackButtonPressed();
    }

    public void Receive(RecipeIngredientEditMessage message)
    {
        ForceDataRefreshOnNextAppearing();
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
