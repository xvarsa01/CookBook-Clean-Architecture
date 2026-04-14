using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeEditViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : RecipeCreateEditBaseViewModel(mediator, navigationService, messengerService)
{
    public RecipeId Id { get; set; } = new(Guid.Empty);
    
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        if (Id.Value != Guid.Empty)
        {
            var result = (await Mediator.Send(new GetRecipeDetailQuery(Id)));
            if (result.IsSuccess)
            {
                Recipe = new RecipeFormModel(result.Value);
            }
        }

        await LoadIngredientsAsync();
    }

    [RelayCommand]
    private async Task AddNewIngredientToRecipeAsync()
    {
        if (IngredientAmountNew.IngredientId == Guid.Empty)
            return;
        
        if (!await ValidateNewIngredientAsync())
            return;

        var request = new RecipeAddIngredientRequest(
            new IngredientId(IngredientAmountNew.IngredientId),
            IngredientAmount.CreateObject(IngredientAmountNew.Amount).Value,
            IngredientAmountNew.Unit);
        
        var result = await Mediator.Send(new AddIngredientToRecipeCommand(new RecipeId(Recipe.Id), request));
        if (result.IsSuccess)
        {
            IngredientAmountNew.RecipeIngredientId =  result.Value.Value;
            Recipe.Ingredients.Add(IngredientAmountNew);

            IngredientAmountNew = RecipeIngredientListModel.Empty;
            SelectedNewIngredient = null;

            MessengerService.Send(new RecipeIngredientAddMessage());
        }
    }
    
    [RelayCommand]
    private async Task UpdateIngredientAsync(RecipeIngredientListModel? model)
    {
        // TODO: add changed models to list and then save it explicitly using button?
        if (model is not null)
        {
            var updateRequest = new RecipeUpdateIngredientRequest(new RecipeIngredientId(model.RecipeIngredientId), IngredientAmount.CreateObject(model.Amount).Value, model.Unit);
            await Mediator.Send(new UpdateIngredientInRecipeCommand(Id, updateRequest));
            MessengerService.Send(new RecipeIngredientEditMessage());
        }
    }
    
    [RelayCommand]
    private async Task RemoveIngredientAsync(RecipeIngredientListModel model)
    {
        await Mediator.Send(new RemoveIngredientFromRecipeCommand(Id, new RecipeIngredientId(model.RecipeIngredientId)));
        Recipe.Ingredients.Remove(model);

        MessengerService.Send(new RecipeIngredientDeleteMessage());
    }

    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        if (!await ValidateRecipeAsync())
            return;

        var imageUrl = TryCreateImageUrl();

        var request = new RecipeUpdateRequest(
            new RecipeId(Recipe.Id),
            RecipeName.CreateObject(Recipe.Name).Value,
            Recipe.Description,
            imageUrl,
            RecipeDuration.CreateObject(Recipe.Duration).Value,
            Recipe.RecipeType);

        await Mediator.Send(new UpdateRecipeCommand(request));

        MessengerService.Send(new RecipeEditMessage { RecipeId = Recipe.Id });

        NavigationService.SendBackButtonPressed();
    }
}
