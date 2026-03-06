using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.Clean.App.Messages;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.UseCases.Ingredient;
using CookBook.Clean.UseCases.Ingredient.Create;
using CookBook.Clean.UseCases.Ingredient.Get;
using CookBook.Clean.UseCases.Ingredient.Update;
using MediatR;

namespace CookBook.Clean.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class IngredientEditViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public partial IngredientDetailModel Ingredient { get; set; } = IngredientDetailModel.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await _mediator.Send(new GetIngredientUseCase(Id)));
        if (result.Success && result.Value is not null)
        {
            Ingredient = result.Value.Ingredient;
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
