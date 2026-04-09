using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Commands.Recipes;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using MediatR;

public partial class RecipeCreateViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : RecipeCreateEditBaseViewModel(mediator, navigationService, messengerService)
{
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        await LoadIngredientsAsync();
    }

    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        if (!await ValidateRecipeAsync())
            return;

        var imageUrl = TryCreateImageUrl();

        var request = new RecipeCreateRequest(
            RecipeName.CreateObject(Recipe.Name).Value,
            Recipe.Description,
            imageUrl,
            RecipeDuration.CreateObject(Recipe.Duration).Value,
            Recipe.RecipeType);

        await Mediator.Send(new CreateRecipeCommand(request));

        MessengerService.Send(new RecipeEditMessage { RecipeId = Recipe.Id });

        NavigationService.SendBackButtonPressed();
    }
}
