using CookBook.CleanArch.Application.Ingredients.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class RecipeCreateViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : RecipeFormBaseViewModel(mediator, navigationService, messengerService)
{
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        await LoadIngredientsAsync();
    }
    
    [RelayCommand]
    private async Task AddNewIngredientToRecipeAsync()
    {
        if (!await ValidateNewIngredientAsync())
            return;
        
        Recipe.Ingredients.Add(IngredientAmountNew);
        OnPropertyChanged(nameof(Recipe));
        await ValidateRecipeAsync();
        
        IngredientAmountNew = RecipeIngredientListModel.Empty;
        SelectedNewIngredient = null;
    }
    
    [RelayCommand]
    private void RemoveIngredient(RecipeIngredientListModel model)
    {
        Recipe.Ingredients.Remove(model);
        OnPropertyChanged(nameof(Recipe));
        MessengerService.Send(new RecipeIngredientDeleteMessage());
    }

    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        if (!await ValidateRecipeAsync())
            return;

        var imageUrl = TryCreateImageUrl();
        var ingredients = MapIngredients();

        var request = new RecipeCreateRequest(
            RecipeName.CreateObject(Recipe.Name).Value,
            Recipe.Description,
            imageUrl,
            RecipeDuration.CreateObject(Recipe.Duration).Value,
            Recipe.RecipeType,
            ingredients);

        await Mediator.Send(new CreateRecipeCommand(request));

        MessengerService.Send(new RecipeEditMessage { RecipeId = Recipe.Id });

        NavigationService.SendBackButtonPressed();
    }
    
    
    private List<RecipeCreateIngredientRequest>? MapIngredients()
    {
        if (Recipe.Ingredients.Count == 0)
            return null;
        
        return Recipe.Ingredients.Select(ingredient => new RecipeCreateIngredientRequest(
            new IngredientId(ingredient.IngredientId),
            IngredientAmount.CreateObject(ingredient.Amount).Value,
            ingredient.Unit)).ToList();
    }
}
