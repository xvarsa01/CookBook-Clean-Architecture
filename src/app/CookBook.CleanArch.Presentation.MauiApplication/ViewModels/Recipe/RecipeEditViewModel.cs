using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Commands.Recipes;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

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

        var result = (await _mediator.Send(new GetRecipeDetailQuery(Id)));
        if (result.IsSuccess && result.Value is not null)
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
            await _mediator.Send(new CreateRecipeCommand(Recipe.Name, Recipe.Description, Recipe.ImageUrl, Recipe.Duration, Recipe.Type));
        }
        else
        {
            await _mediator.Send(new UpdateRecipeCommand(Recipe.Id, Recipe.Name, Recipe.Description, Recipe.ImageUrl, Recipe.Duration, Recipe.Type));
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
