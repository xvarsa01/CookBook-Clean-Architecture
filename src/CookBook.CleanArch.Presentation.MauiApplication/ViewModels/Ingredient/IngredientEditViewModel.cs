using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Application.Ingredients.Queries;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using FluentValidation.Results;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class IngredientEditViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : IngredientFormBaseViewModel(mediator, navigationService, messengerService)
{
    public IngredientId Id { get; set; } = new(Guid.Empty);

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        if (Id.Value == Guid.Empty)
        {
            return;
        }

        var result = (await Mediator.Send(new GetIngredientDetailQuery(Id)));
        if (result.IsSuccess)
        {
            Ingredient = new IngredientFormModel(result.Value);
        }
    }
    
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!await ValidateAsync())
            return;

        var imageUrl = TryCreateImageUrl();

        var request = new IngredientUpdateRequest(
            Id,
            Ingredient.Name,
            Ingredient.Description,
            imageUrl);

        await Mediator.Send(new UpdateIngredientCommand(request));

        MessengerService.Send(new IngredientEditMessage { IngredientId = Id });

        NavigationService.SendBackButtonPressed();
    }
}
