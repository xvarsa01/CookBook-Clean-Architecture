using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Commands.Recipes;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Validations;
using FluentValidation.Results;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeEditViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<RecipeIngredientEditMessage>, IRecipient<RecipeIngredientAddMessage>,
        IRecipient<RecipeIngredientDeleteMessage>
{
    public RecipeId Id { get; set; } = new(Guid.Empty);

    [ObservableProperty]
    public partial bool IngredientsEditingIsActive { get; set; }
    public bool IsExistingRecipe => Recipe.Id != Guid.Empty;
    public string ToggleEditIngredientButtonText => IngredientsEditingIsActive
        ? RecipeEditViewTexts.EditIngredients_StopEdit_Button_Text
        : RecipeEditViewTexts.EditIngredients_StartEdit_Button_Text;

    [ObservableProperty]
    public partial RecipeDetailModel Recipe { get; set; } = RecipeDetailModel.Empty;
    
    public List<RecipeType> FoodTypes { get; set; } = [.. Enum.GetValues<RecipeType>()];
    public List<MeasurementUnit> Units { get; set; } = [.. Enum.GetValues<MeasurementUnit>()];

    [ObservableProperty]
    public partial RecipeIngredientListModel IngredientAmountNew { get; set; } = RecipeIngredientListModel.Empty;

    [ObservableProperty]
    public partial ObservableCollection<IngredientListModel> Ingredients { get; set; } = [];

    public IngredientListModel? SelectedNewIngredient
    {
        get;
        set
        {
            if (!SetProperty(ref field, value) || value is null)
            {
                return;
            }

            IngredientAmountNew.IngredientId = value.Id;
            IngredientAmountNew.IngredientName = value.Name;
            IngredientAmountNew.IngredientImageUrl = value.ImageUrl;
        }
    }


    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        if (Id.Value != Guid.Empty)
        {
            var result = (await mediator.Send(new GetRecipeDetailQuery(Id)));
            if (result.IsSuccess)
            {
                Recipe = RecipeDetailModel.MapFromResponse(result.Value);
            }
        }

        await LoadIngredients();
    }

    private async Task LoadIngredients()
    {
        IngredientFilter filter = new();
        var result = await mediator.Send(new GetIngredientListQuery(filter));
        if (!result.IsSuccess)
        {
            return;
        }

        Ingredients ??= [];
        Ingredients.Clear();
        foreach (var item in result.Value.Items)
        {
            Ingredients.Add(IngredientListModel.MapFromResponse(item));
        }
    }

    [RelayCommand]
    private void ToggleEditingOfRecipeIngredient() => IngredientsEditingIsActive = !IngredientsEditingIsActive;

    [RelayCommand]
    private async Task AddNewIngredientToRecipeAsync()
    {
        if (IngredientAmountNew.IngredientId == Guid.Empty)
            return;
        
        var validator = new RecipeIngredientListModelValidator();
        IngredientAmountNew.ValidationResults = await validator.ValidateAsync(IngredientAmountNew);
        if (!IngredientAmountNew.ValidationResults.IsValid)
        {
            // If there are errors, do not continue
            return;
        }

        var ingredientId = new IngredientId(IngredientAmountNew.IngredientId);
        var request = new RecipeAddIngredientRequest(ingredientId, IngredientAmount.CreateObject(IngredientAmountNew.Amount).Value, IngredientAmountNew.Unit);
        var result = await mediator.Send(new AddIngredientToRecipeCommand(new RecipeId(Recipe.Id), request));
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
            await mediator.Send(new UpdateIngredientInRecipeCommand(Id, updateRequest));
            MessengerService.Send(new RecipeIngredientEditMessage());
        }
    }
    
    [RelayCommand]
    private async Task RemoveIngredientAsync(RecipeIngredientListModel model)
    {
        await mediator.Send(new RemoveIngredientFromRecipeCommand(Id, new RecipeIngredientId(model.RecipeIngredientId)));
        Recipe.Ingredients.Remove(model);

        MessengerService.Send(new RecipeIngredientDeleteMessage());
    }

    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        var validator = new RecipeDetailModelValidator();
        Recipe.ValidationResults = await validator.ValidateAsync(Recipe);
        if (!Recipe.ValidationResults.IsValid)
        {
            // If there are errors, do not continue
            return;
        }
        
        var imageUrl = string.IsNullOrEmpty(Recipe.ImageUrl)
            ? null
            : ImageUrl.CreateObject(Recipe.ImageUrl).Value;
            
        if (Recipe.Id == Guid.Empty)
        {
            var request = new RecipeCreateRequest(RecipeName.CreateObject(Recipe.Name).Value, Recipe.Description, imageUrl, RecipeDuration.CreateObject(Recipe.Duration).Value, Recipe.RecipeType);
            await mediator.Send(new CreateRecipeCommand(request));
        }
        else
        {
            var request = new RecipeUpdateRequest(new RecipeId(Recipe.Id), RecipeName.CreateObject(Recipe.Name).Value, Recipe.Description, imageUrl, RecipeDuration.CreateObject(Recipe.Duration).Value, Recipe.RecipeType);
            await mediator.Send(new UpdateRecipeCommand(request));
        }

        MessengerService.Send(new RecipeEditMessage { RecipeId = Recipe.Id});

        navigationService.SendBackButtonPressed();
    }
    
    [RelayCommand]
    private void ValidateProperty(string propertyName)
    {
        var validator = new RecipeDetailModelValidator();

        ValidationResult result = validator.Validate(Recipe);

        Recipe.ValidationResults = result;
        
        // Remove the previous error message for this property
        Recipe.ValidationResults.Errors.Remove(Recipe.ValidationResults.Errors.FirstOrDefault(x => x.PropertyName == propertyName));
        Recipe.ValidationResults.Errors.AddRange(result.Errors);

        // Notify the UI that the property has changed
        OnPropertyChanged(nameof(Recipe.ValidationResults));
    }
    
    [RelayCommand]
    private void ValidateIngredientAmountNewProperty(string propertyName)
    {
        var validator = new RecipeIngredientListModelValidator();

        ValidationResult result = validator.Validate(IngredientAmountNew);

        IngredientAmountNew.ValidationResults = result;
        
        // Remove the previous error message for this property
        IngredientAmountNew.ValidationResults.Errors.Remove(IngredientAmountNew.ValidationResults.Errors.FirstOrDefault(x => x.PropertyName == propertyName));
        IngredientAmountNew.ValidationResults.Errors.AddRange(result.Errors);

        // Notify the UI that the property has changed
        OnPropertyChanged(nameof(Recipe.ValidationResults));
    }

    partial void OnRecipeChanged(RecipeDetailModel value)
    {
        OnPropertyChanged(nameof(IsExistingRecipe));
    }
    
    public void Receive(RecipeIngredientEditMessage message)
    {
        ForceDataRefreshOnNextAppearing();
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
