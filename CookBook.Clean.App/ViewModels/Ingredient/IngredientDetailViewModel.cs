using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.App.Messages;
using CookBook.Clean.App.Resources.Texts;
using CookBook.Clean.App.Services;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.UseCases.Ingredient;
using CookBook.Clean.UseCases.Ingredient.Delete;
using CookBook.Clean.UseCases.Ingredient.Get;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class IngredientDetailViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService)
    : ViewModelBase(messengerService), IRecipient<IngredientEditMessage>
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public partial IngredientDetailModel? Ingredient { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await _mediator.Send(new GetIngredientUseCase(Id)));
        if (result.Success && result.Value is not null)
        {
            Ingredient = result.Value;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Ingredient is not null)
        {
            var result = await _mediator.Send(new DeleteIngredientUseCase(Ingredient.Id));
            if (!result.Success)
            {
                await alertService.DisplayAsync(IngredientDetailViewModelTexts.DeleteError_Alert_Title, IngredientDetailViewModelTexts.DeleteError_Alert_Message);
                return;
            }
            MessengerService.Send(new IngredientDeleteMessage());
            navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if(Ingredient?.Id is not null)
        {
            await navigationService.GoToAsync(NavigationService.IngredientEditRouteRelative,
                new Dictionary<string, object?> { [nameof(IngredientEditViewModel.Id)] = Ingredient.Id });
        }
    }

    public void Receive(IngredientEditMessage message)
    {
        if (message.IngredientId == Ingredient?.Id)
        {
            ForceDataRefreshOnNextAppearing();
        }
    }
}
