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
    private readonly List<PendingAddIngredientChange> _pendingAddedIngredients = [];
    private readonly List<RecipeUpdateIngredientRequest> _pendingUpdatedIngredients = [];
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

        _pendingAddedIngredients.Add(new PendingAddIngredientChange(
            model,
            new RecipeAddIngredientRequest(
                new IngredientId(model.IngredientId),
                ingredientAmountResult.Value,
                model.Unit)));

        IngredientAmountNew = RecipeIngredientListModel.Empty;
        SelectedNewIngredient = null;

        MessengerService.Send(new RecipeIngredientAddMessage());
    }
    
    [RelayCommand]
    private void UpdateIngredientAsync(RecipeIngredientListModel? model)
    {
        if (model is null)
            return;

        var ingredientAmountResult = IngredientAmount.CreateObject(model.Amount);
        if (ingredientAmountResult.IsFailure)
            return;

        var pendingAdd = _pendingAddedIngredients.FirstOrDefault(x => ReferenceEquals(x.Model, model));
        if (pendingAdd is not null)
        {
            pendingAdd.Request = new RecipeAddIngredientRequest(
                new IngredientId(model.IngredientId),
                ingredientAmountResult.Value,
                model.Unit);
            MessengerService.Send(new RecipeIngredientEditMessage());
            return;
        }

        if (model.RecipeIngredientId == Guid.Empty)
            return;

        if (_pendingRemovedIngredientIds.Contains(model.RecipeIngredientId))
            return;

        var updateRequest = new RecipeUpdateIngredientRequest(
            new RecipeIngredientId(model.RecipeIngredientId),
            ingredientAmountResult.Value,
            model.Unit);
        var existingUpdateIndex = _pendingUpdatedIngredients.FindIndex(x => x.EntryId.Value == model.RecipeIngredientId);
        if (existingUpdateIndex >= 0)
        {
            _pendingUpdatedIngredients[existingUpdateIndex] = updateRequest;
        }
        else
        {
            _pendingUpdatedIngredients.Add(updateRequest);
        }
        MessengerService.Send(new RecipeIngredientEditMessage());
    }
    
    [RelayCommand]
    private async Task RemoveIngredientAsync(RecipeIngredientListModel model)
    {
        Recipe.Ingredients.Remove(model);
        OnPropertyChanged(nameof(Recipe));
        await ValidateRecipeAsync();

        var pendingAdd = _pendingAddedIngredients.FirstOrDefault(x => ReferenceEquals(x.Model, model));
        if (pendingAdd is not null)
        {
            _pendingAddedIngredients.Remove(pendingAdd);
        }
        else
        {
            if (model.RecipeIngredientId != Guid.Empty)
            {
                _pendingUpdatedIngredients.RemoveAll(x => x.EntryId.Value == model.RecipeIngredientId);
                if (!_pendingRemovedIngredientIds.Contains(model.RecipeIngredientId))
                {
                    _pendingRemovedIngredientIds.Add(model.RecipeIngredientId);
                }
            }
        }

        MessengerService.Send(new RecipeIngredientDeleteMessage());
    }

    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        if (!await ValidateRecipeAsync())
            return;

        if (!await ApplyPendingIngredientChangesAsync())
            return;

        var imageUrl = TryCreateImageUrl();

        var request = new RecipeUpdateRequest(
            new RecipeId(Recipe.Id),
            RecipeName.CreateObject(Recipe.Name).Value,
            Recipe.Description,
            imageUrl,
            RecipeDuration.CreateObject(Recipe.Duration).Value,
            Recipe.RecipeType);

        var updateResult = await Mediator.Send(new UpdateRecipeCommand(request));
        if (!updateResult.IsSuccess)
            return;

        MessengerService.Send(new RecipeEditMessage { RecipeId = Recipe.Id });

        NavigationService.SendBackButtonPressed();
    }

    private async Task<bool> ApplyPendingIngredientChangesAsync()
    {
        foreach (var recipeIngredientId in _pendingRemovedIngredientIds)
        {
            var result = await Mediator.Send(new RemoveIngredientFromRecipeByEntryIdCommand(Id, new RecipeIngredientId(recipeIngredientId)));
            if (!result.IsSuccess)
                return false;
        }

        foreach (var pendingAdd in _pendingAddedIngredients)
        {
            var result = await Mediator.Send(new AddIngredientToRecipeCommand(Id, pendingAdd.Request));
            if (!result.IsSuccess)
                return false;
        }

        foreach (var request in _pendingUpdatedIngredients)
        {
            var result = await Mediator.Send(new UpdateIngredientInRecipeCommand(Id, request));
            if (!result.IsSuccess)
                return false;
        }

        _pendingRemovedIngredientIds.Clear();
        _pendingAddedIngredients.Clear();
        _pendingUpdatedIngredients.Clear();

        return true;
    }

    private sealed class PendingAddIngredientChange(RecipeIngredientListModel model, RecipeAddIngredientRequest request)
    {
        public RecipeIngredientListModel Model { get; } = model;
        public RecipeAddIngredientRequest Request { get; set; } = request;
    }
}
