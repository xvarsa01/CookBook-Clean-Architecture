using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Commands.Ingredients;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
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
    public partial IngredientDetailModel? Ingredient { get; set; }
    
    [ObservableProperty]
    public partial ObservableCollection<RecipeListModel> RecipesUsingIngredient { get; set; } = [];

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await mediator.Send(new GetIngredientDetailQuery(Id)));
        if (!result.IsSuccess)
        {
            return;
        }

        Ingredient = IngredientDetailModel.MapFromResponse(result.Value);
        
        var recipeResult = await mediator.Send(new GetRecipeListByContainingIngredientIdQuery(Id));
        if (recipeResult.IsSuccess)
        {
            foreach (var item in recipeResult.Value)
            {
                RecipesUsingIngredient.Add(RecipeListModel.MapFromResponse(item));
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
    private async Task GoToRecipeDetailAsync(Guid id)
        => await navigationService.GoToAsync($"{NavigationService.RecipeListRouteAbsolute}{NavigationService.RecipeDetailRouteRelative}",
            new Dictionary<string, object?>
            {
                [nameof(RecipeDetailViewModel.Id)] = new RecipeId(id)
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
