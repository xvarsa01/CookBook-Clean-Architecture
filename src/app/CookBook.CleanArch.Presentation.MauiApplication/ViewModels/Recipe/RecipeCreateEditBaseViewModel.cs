using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Validations;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public abstract partial class RecipeCreateEditBaseViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
        IRecipient<RecipeIngredientEditMessage>,
        IRecipient<RecipeIngredientAddMessage>,
        IRecipient<RecipeIngredientDeleteMessage>
{
    protected readonly IMediator Mediator = mediator;
    protected readonly INavigationService NavigationService = navigationService;

    protected readonly RecipeDetailModelValidator RecipeValidator = new();
    protected readonly RecipeIngredientListModelValidator IngredientValidator = new();

    [ObservableProperty]
    public partial bool IngredientsEditingIsActive { get; set; }

    public string ToggleEditIngredientButtonText => IngredientsEditingIsActive
        ? RecipeEditViewTexts.EditIngredients_StopEdit_Button_Text
        : RecipeEditViewTexts.EditIngredients_StartEdit_Button_Text;

    [ObservableProperty]
    public partial RecipeDetailModel Recipe { get; set; } = RecipeDetailModel.Empty;

    public List<RecipeType> FoodTypes { get; } = [.. Enum.GetValues<RecipeType>()];
    public List<MeasurementUnit> Units { get; } = [.. Enum.GetValues<MeasurementUnit>()];

    [ObservableProperty]
    public partial RecipeIngredientListModel IngredientAmountNew { get; set; } = RecipeIngredientListModel.Empty;

    [ObservableProperty]
    public partial ObservableCollection<IngredientListResponse> Ingredients { get; set; } = [];

    public IngredientListResponse? SelectedNewIngredient
    {
        get;
        set
        {
            if (!SetProperty(ref field, value) || value is null)
                return;

            IngredientAmountNew.IngredientId = value.Id.Value;
            IngredientAmountNew.IngredientName = value.Name;
            IngredientAmountNew.IngredientImageUrl = value.ImageUrl?.Value;
        }
    }

    protected async Task LoadIngredientsAsync()
    {
        var result = await Mediator.Send(new GetIngredientListQuery(new IngredientFilter()));
        if (!result.IsSuccess)
            return;

        Ingredients.Clear();
        foreach (var item in result.Value.Items)
            Ingredients.Add(item);
    }

    protected async Task<bool> ValidateRecipeAsync()
    {
        Recipe.ValidationResults = await RecipeValidator.ValidateAsync(Recipe);
        return Recipe.ValidationResults.IsValid;
    }

    protected async Task<bool> ValidateNewIngredientAsync()
    {
        IngredientAmountNew.ValidationResults = await IngredientValidator.ValidateAsync(IngredientAmountNew);
        return IngredientAmountNew.ValidationResults.IsValid;
    }

    protected ImageUrl? TryCreateImageUrl()
    {
        if (string.IsNullOrEmpty(Recipe.ImageUrl))
            return null;

        var result = ImageUrl.CreateObject(Recipe.ImageUrl);
        return result.IsSuccess ? result.Value : null;
    }

    [RelayCommand]
    private void ToggleEditingOfRecipeIngredient()
        => IngredientsEditingIsActive = !IngredientsEditingIsActive;

    [RelayCommand]
    private async Task ValidateProperty(string propertyName)
    {
        var result = await RecipeValidator.ValidateAsync(Recipe);
        Recipe.ValidationResults = result;

        Recipe.ValidationResults.Errors.Remove(
            Recipe.ValidationResults.Errors.FirstOrDefault(x => x.PropertyName == propertyName));

        Recipe.ValidationResults.Errors.AddRange(result.Errors);

        OnPropertyChanged(nameof(Recipe.ValidationResults));
    }

    [RelayCommand]
    private async Task ValidateIngredientAmountNewProperty(string propertyName)
    {
        var result = await IngredientValidator.ValidateAsync(IngredientAmountNew);
        IngredientAmountNew.ValidationResults = result;

        IngredientAmountNew.ValidationResults.Errors.Remove(
            IngredientAmountNew.ValidationResults.Errors.FirstOrDefault(x => x.PropertyName == propertyName));

        IngredientAmountNew.ValidationResults.Errors.AddRange(result.Errors);

        OnPropertyChanged(nameof(IngredientAmountNew.ValidationResults));
    }

    public void Receive(RecipeIngredientEditMessage message) => ForceDataRefreshOnNextAppearing();
    public void Receive(RecipeIngredientAddMessage message) => ForceDataRefreshOnNextAppearing();
    public void Receive(RecipeIngredientDeleteMessage message) => ForceDataRefreshOnNextAppearing();

    partial void OnRecipeChanged(RecipeDetailModel value)
    {
        OnPropertyChanged(nameof(IsExistingRecipe));
    }

    public bool IsExistingRecipe => Recipe.Id != Guid.Empty;
}
