// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.Clean.App.Messages;
using CookBook.Clean.App.Services;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.Recipe;
using CookBook.Clean.UseCases.Recipe.Delete;
using CookBook.Clean.UseCases.Recipe.Get;
using MediatR;

namespace CookBook.Clean.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeDetailViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<RecipeEditMessage>, IRecipient<RecipeIngredientAddMessage>,
        IRecipient<RecipeIngredientDeleteMessage>
{
    public Guid Id { get; set; }

    [ObservableProperty]
    public partial RecipeDetailModel? Recipe { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await _mediator.Send(new GetRecipeUseCase(Id)));
        if (result.Success && result.Value is not null)
        {
            Recipe = result.Value.Recipe;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Recipe is not null)
        {
            var result = (await _mediator.Send(new DeleteRecipeUseCase(Recipe.Id)));

            MessengerService.Send(new RecipeDeleteMessage());

            navigationService.SendBackButtonPressed();
        }
    }


    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if (Recipe is not null)
        {
            await navigationService.GoToAsync(NavigationService.RecipeEditRouteRelative,
                new Dictionary<string, object?>
                {
                    [nameof(RecipeEditViewModel.Id)] = Recipe.Id
                }
            );
        }
    }

    public void Receive(RecipeEditMessage message)
    {
        if (message.RecipeId == Recipe?.Id)
        {
            ForceDataRefreshOnNextAppearing();
        }
    }

    public void Receive(RecipeIngredientAddMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(RecipeIngredientDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}
