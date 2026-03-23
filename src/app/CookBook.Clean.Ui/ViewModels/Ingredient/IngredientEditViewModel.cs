using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Application.UseCases.Ingredients;
using CookBook.Clean.Ui.Messages;
using CookBook.Clean.Ui.Services.Interfaces;
using MediatR;

namespace CookBook.Clean.Ui.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class IngredientEditViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public Guid Id { get; set; } = Guid.Empty;

    [ObservableProperty]
    public partial IngredientDetailModel Ingredient { get; set; } = IngredientDetailModel.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        if (Id == Guid.Empty)
        {
            return;
        }
        
        var result = (await _mediator.Send(new GetIngredientDetailQuery(Id)));
        if (result.IsSuccess && result.Value is not null)
        {
            Ingredient = result.Value;
        }
        else
        {
            Ingredient = IngredientDetailModel.Empty;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Ingredient.Id == Guid.Empty)
        {
            await _mediator.Send(new CreateIngredientUseCase(Ingredient.Name, Ingredient.Description, Ingredient.ImageUrl));
        }
        else
        {
            await _mediator.Send(new UpdateIngredientUseCase(Ingredient.Id, Ingredient.Name, Ingredient.Description, Ingredient.ImageUrl));
        }

        MessengerService.Send(new IngredientEditMessage { IngredientId = Ingredient.Id });

        navigationService.SendBackButtonPressed();
    }
}
