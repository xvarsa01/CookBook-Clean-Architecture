// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.App.Messages;
using CookBook.Clean.App.Services;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.UseCases.Ingredient;
using CookBook.Clean.UseCases.Ingredient.GetList;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.App.ViewModels;

public partial class IngredientListViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<IngredientEditMessage>, IRecipient<IngredientDeleteMessage>
{
    [ObservableProperty]
    public partial IEnumerable<IngredientListModel> Ingredients { get; set; } = [];

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await _mediator.Send(new GetListIngredientUseCase())).Value;
        if (result is not null)
        {
            Ingredients = result;
        }
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync(NavigationService.IngredientEditRouteRelative);
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync(NavigationService.IngredientDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(IngredientDetailViewModel.Id)] = id
            }
        );
    }

    public void Receive(IngredientEditMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(IngredientDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}
