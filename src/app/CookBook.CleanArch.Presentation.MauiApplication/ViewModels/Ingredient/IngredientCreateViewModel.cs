using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Commands.Ingredients;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class IngredientCreateViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : IngredientCreateEditBaseViewModel(mediator, navigationService, messengerService)
{
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!await ValidateAsync())
            return;

        var imageUrl = TryCreateImageUrl();
        if (Ingredient.ImageUrl is not null && imageUrl is null)
            return;

        var request = new IngredientCreateRequest(
            Ingredient.Name,
            Ingredient.Description,
            imageUrl);

        await Mediator.Send(new CreateIngredientCommand(request));

        NavigationService.SendBackButtonPressed();
    }
}
