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
    private RecipeResponse? _recipeResponse = null;
    
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
    private async Task UpdateIngredientAsync(RecipeIngredientListModel? model)
    {
        if (model is null)
            return;
        
        if (!await ValidateExistingIngredientAsync(model))
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

    private async Task<bool> ApplyPendingIngredientChangesAsync()
    {
        const int minIngredients = Domain.Recipes.Recipe.MinIngredients;
        const int maxIngredients = Domain.Recipes.Recipe.MaxIngredients;

        var pendingAdds = _pendingAddedIngredients.Count;
        var pendingRemoves = _pendingRemovedIngredientIds.Count;
        var finalCount = Recipe.Ingredients.Count;
        var baseCount = finalCount - pendingAdds + pendingRemoves;

        if (finalCount < minIngredients || finalCount > maxIngredients)
            return false;

        // Updates do not affect count, apply them first.
        foreach (var request in _pendingUpdatedIngredients)
        {
            var result = await Mediator.Send(new UpdateIngredientInRecipeCommand(Id, request));
            if (!result.IsSuccess)
                return false;
        }

        var adds = _pendingAddedIngredients.ToList();
        var removes = _pendingRemovedIngredientIds.ToList();
        var currentCount = baseCount;

        while (adds.Count > 0 || removes.Count > 0)
        {
            if (currentCount >= maxIngredients && removes.Count > 0)
            {
                var removeId = removes[0];
                removes.RemoveAt(0);

                var result = await Mediator.Send(
                    new RemoveIngredientFromRecipeByEntryIdCommand(Id, new RecipeIngredientId(removeId)));
                if (!result.IsSuccess)
                    return false;

                currentCount--;
                continue;
            }

            if (currentCount <= minIngredients && adds.Count > 0)
            {
                var add = adds[0];
                adds.RemoveAt(0);

                var result = await Mediator.Send(new AddIngredientToRecipeCommand(Id, add.Request));
                if (!result.IsSuccess)
                    return false;

                currentCount++;
                continue;
            }

            if (adds.Count > 0)
            {
                var add = adds[0];
                adds.RemoveAt(0);

                var result = await Mediator.Send(new AddIngredientToRecipeCommand(Id, add.Request));
                if (!result.IsSuccess)
                    return false;

                currentCount++;
                continue;
            }

            if (removes.Count > 0)
            {
                var removeId = removes[0];
                removes.RemoveAt(0);

                var result = await Mediator.Send(
                    new RemoveIngredientFromRecipeByEntryIdCommand(Id, new RecipeIngredientId(removeId)));
                if (!result.IsSuccess)
                    return false;

                currentCount--;
            }
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
