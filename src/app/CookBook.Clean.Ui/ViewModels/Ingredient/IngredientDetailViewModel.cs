using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Queries.Ingredients;
using CookBook.Clean.Ui.Messages;
using CookBook.Clean.Ui.Resources.Texts;
using CookBook.Clean.Ui.Services;
using CookBook.Clean.Ui.Services.Interfaces;
using MediatR;

namespace CookBook.Clean.Ui.ViewModels;

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

        var result = (await _mediator.Send(new GetIngredientDetailQuery(Id)));
        if (result.IsSuccess && result.Value is not null)
        {
            Ingredient = result.Value;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Ingredient is not null)
        {
            var result = await _mediator.Send(new DeleteIngredientCommand(Ingredient.Id));
            if (!result.IsSuccess)
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
