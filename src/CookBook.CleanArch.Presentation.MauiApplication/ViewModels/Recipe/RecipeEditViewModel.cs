using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeEditViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : RecipeFormBaseViewModel(mediator, navigationService, messengerService)
{
    private RecipeResponse? _recipeResponse;
    
    private readonly List<RecipeIngredientListModel> _pendingAddedIngredients = [];
    private readonly List<RecipeIngredientListModel> _pendingUpdatedIngredients = [];
    private readonly List<Guid> _pendingRemovedIngredientIds = [];

    public RecipeId Id { get; set; } = new(Guid.Empty);
    
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        _pendingAddedIngredients.Clear();
        _pendingUpdatedIngredients.Clear();
        _pendingRemovedIngredientIds.Clear();

        if (Id.Value != Guid.Empty)
        {
            var result = (await Mediator.Send(new GetRecipeDetailQuery(Id)));
            if (result.IsSuccess)
            {
                _recipeResponse = result.Value;
                Recipe = new RecipeFormModel(_recipeResponse);
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

        var ingredientAmountResult = IngredientAmount.CreateObject(IngredientAmountNew.Amount);
        if (ingredientAmountResult.IsFailure)
            return;

        var model = new RecipeIngredientListModel
        {
            RecipeIngredientId = Guid.Empty,
            IngredientId = IngredientAmountNew.IngredientId,
            IngredientName = IngredientAmountNew.IngredientName,
            IngredientImageUrl = IngredientAmountNew.IngredientImageUrl,
            Amount = IngredientAmountNew.Amount,
            Unit = IngredientAmountNew.Unit
        };

        Recipe.Ingredients.Add(model);
        OnPropertyChanged(nameof(Recipe));
        await ValidateRecipeAsync();

        _pendingAddedIngredients.Add(model);

        IngredientAmountNew = RecipeIngredientListModel.Empty;
        SelectedNewIngredient = null;

        MessengerService.Send(new RecipeIngredientAddMessage());
    }
    
    [RelayCommand]
    private async Task UpdateIngredientAsync(RecipeIngredientListModel? model)
    {
        if (model is null || model.RecipeIngredientId == Guid.Empty || _pendingRemovedIngredientIds.Contains(model.RecipeIngredientId))
            return;
        
        if (!await ValidateExistingIngredientAsync(model))
            return;

        var ingredientAmountResult = IngredientAmount.CreateObject(model.Amount);
        if (ingredientAmountResult.IsFailure)
            return;

        var pendingAdd = _pendingAddedIngredients.FirstOrDefault(x => ReferenceEquals(x, model));
        if (pendingAdd is not null)
        {
            MessengerService.Send(new RecipeIngredientEditMessage());
            return;
        }
        
        var existingUpdateIndex = _pendingUpdatedIngredients.FindIndex(x => x.RecipeIngredientId == model.RecipeIngredientId);
        if (existingUpdateIndex >= 0)
        {
            _pendingUpdatedIngredients[existingUpdateIndex] = model;
        }
        else
        {
            _pendingUpdatedIngredients.Add(model);
        }
        MessengerService.Send(new RecipeIngredientEditMessage());
    }
    
    [RelayCommand]
    private async Task RemoveIngredientAsync(RecipeIngredientListModel model)
    {
        if (model.RecipeIngredientId == Guid.Empty)
            return;
        
        Recipe.Ingredients.Remove(model);
        OnPropertyChanged(nameof(Recipe));
        await ValidateRecipeAsync();

        var pendingAdd = _pendingAddedIngredients.FirstOrDefault(x => ReferenceEquals(x, model));
        if (pendingAdd is not null)
        {
            _pendingAddedIngredients.Remove(pendingAdd);
            return;
        }

        _pendingUpdatedIngredients.RemoveAll(x => x.RecipeIngredientId == model.RecipeIngredientId);
        if (!_pendingRemovedIngredientIds.Contains(model.RecipeIngredientId))
        {
            _pendingRemovedIngredientIds.Add(model.RecipeIngredientId);
        }

        MessengerService.Send(new RecipeIngredientDeleteMessage());
    }

    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        if (!await ValidateRecipeAsync())
            return;

        var imageUrl = TryCreateImageUrl();

        var additionsRequestList = _pendingAddedIngredients.Select(x => new RecipeUpdateWithIngredientsAddIngredientRequest(
            new IngredientId(x.IngredientId),
            IngredientAmount.CreateObject(x.Amount).Value,
            x.Unit)).ToList();

        var updatesRequestList = _pendingUpdatedIngredients.Select(x =>
            new RecipeUpdateWithIngredientsUpdateIngredientRequest(
                new RecipeIngredientId(x.RecipeIngredientId),
                IngredientAmount.CreateObject(x.Amount).Value,
                x.Unit)).ToList();
        
        var removalsRequestList = _pendingRemovedIngredientIds.Select(x => new RecipeIngredientId(x)).ToList();
        
        var request = new RecipeUpdateWithIngredientsRequest(
            new RecipeId(Recipe.Id),
            RecipeName.CreateObject(Recipe.Name).Value,
            Recipe.Description,
            imageUrl,
            RecipeDuration.CreateObject(Recipe.Duration).Value,
            Recipe.RecipeType,
            additionsRequestList,
            updatesRequestList,
            removalsRequestList
            );

        var updateResult = await Mediator.Send(new UpdateRecipeWithIngredientsCommand(request));
        if (!updateResult.IsSuccess)
            return;

        _pendingAddedIngredients.Clear();
        _pendingUpdatedIngredients.Clear();
        _pendingRemovedIngredientIds.Clear();

        MessengerService.Send(new RecipeEditMessage { RecipeId = new RecipeId(Recipe.Id) });

        NavigationService.SendBackButtonPressed();
    }

    [RelayCommand]
    private void CancelChangesAsync()
    {
        _pendingAddedIngredients.Clear();
        _pendingUpdatedIngredients.Clear();
        _pendingRemovedIngredientIds.Clear();

        if (_recipeResponse is not null)
        {
            Recipe = new RecipeFormModel(_recipeResponse!);
        }
    }
}
