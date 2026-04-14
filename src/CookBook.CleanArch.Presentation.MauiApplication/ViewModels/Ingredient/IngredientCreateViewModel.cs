using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class IngredientCreateViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : IngredientFormBaseViewModel(mediator, navigationService, messengerService)
{
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!await ValidateAsync())
            return;

        var imageUrl = TryCreateImageUrl();

        var request = new IngredientCreateRequest(
            Ingredient.Name,
            Ingredient.Description,
            imageUrl);

        await Mediator.Send(new CreateIngredientCommand(request));

        NavigationService.SendBackButtonPressed();
    }
}
