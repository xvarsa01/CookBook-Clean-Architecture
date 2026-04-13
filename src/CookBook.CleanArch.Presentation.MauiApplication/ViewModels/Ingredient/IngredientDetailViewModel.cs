using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Application.Ingredients.Queries;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class IngredientDetailViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService)
    : ViewModelBase(messengerService), IRecipient<IngredientEditMessage>
{
    public IngredientId Id { get; set; }

    [ObservableProperty]
    public partial IngredientDetailResponse? Ingredient { get; set; }
    
    [ObservableProperty]
    public partial ObservableCollection<RecipeListResponse> RecipesUsingIngredient { get; set; } = [];

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await mediator.Send(new GetIngredientDetailQuery(Id)));
        if (!result.IsSuccess)
        {
            return;
        }

        Ingredient = result.Value;
        
        var recipeResult = await mediator.Send(new GetRecipeListByContainingIngredientIdQuery(Id));
        if (recipeResult.IsSuccess)
        {
            RecipesUsingIngredient.Clear();
            foreach (var item in recipeResult.Value)
            {
                RecipesUsingIngredient.Add(item);
            }
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Ingredient is not null)
        {
            var result = await mediator.Send(new DeleteIngredientCommand(Id));
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
        await navigationService.GoToAsync(NavigationService.IngredientEditRouteRelative,
            new Dictionary<string, object?> { [nameof(IngredientEditViewModel.Id)] = Id });
    }

    [RelayCommand]
    private async Task GoToRecipeDetailAsync(RecipeId id)
        => await navigationService.GoToAsync($"{NavigationService.RecipeListRouteAbsolute}{NavigationService.RecipeDetailRouteRelative}",
            new Dictionary<string, object?>
            {
                [nameof(RecipeDetailViewModel.Id)] = id
            }
        );

    public void Receive(IngredientEditMessage message)
    {
        if (message.IngredientId == Id)
        {
            ForceDataRefreshOnNextAppearing();
        }
    }
}
