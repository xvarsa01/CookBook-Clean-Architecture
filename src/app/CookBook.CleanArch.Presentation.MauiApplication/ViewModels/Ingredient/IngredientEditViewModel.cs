using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Commands.Ingredients;
using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class IngredientEditViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public IngredientId Id { get; set; } = new(Guid.Empty);

    [ObservableProperty]
    public partial IngredientDetailModel Ingredient { get; set; } = IngredientDetailModel.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        if (Id.Value == Guid.Empty)
        {
            return;
        }
        
        var result = (await _mediator.Send(new GetIngredientDetailQuery(Id)));
        if (result.IsSuccess)
        {
            Ingredient = IngredientDetailModel.MapFromResponse(result.Value);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var imageUrl = Ingredient.ImageUrl is null
            ? null
            : ImageUrl.CreateObject(Ingredient.ImageUrl);
        
        if (imageUrl != null && imageUrl.IsFailure)
        {
            // show UI error
            return;
        }

        if (string.IsNullOrWhiteSpace(Ingredient.Name))
        {
            // show UI error
            return;
        }
        
        if (Id.Value == Guid.Empty)
        {
            var createRequest = new IngredientCreateRequest(Ingredient.Name, Ingredient.Description, imageUrl?.Value);
            await _mediator.Send(new CreateIngredientCommand(createRequest));
        }
        else
        {
            var updateRequest = new IngredientUpdateRequest(Id, Ingredient.Name, Ingredient.Description, imageUrl?.Value);
            await _mediator.Send(new UpdateIngredientCommand(updateRequest));
        }

        MessengerService.Send(new IngredientEditMessage { IngredientId = Id });

        navigationService.SendBackButtonPressed();
    }
}
