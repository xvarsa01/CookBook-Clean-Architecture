using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Commands.Recipes;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeIngredientsEditViewModel(
    IMediator _mediator,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public RecipeId Id { get; set; }

    public List<MeasurementUnit> Units { get; set; } = [.. Enum.GetValues<MeasurementUnit>()];

    [ObservableProperty]
    public partial RecipeDetailModel Recipe { get; set; } = RecipeDetailModel.Empty;

    [ObservableProperty]
    public partial ObservableCollection<IngredientListModel> Ingredients { get; set; } = [];

    [ObservableProperty]
    public partial RecipeIngredientListModel? IngredientSelected { get; set; }

    [ObservableProperty]
    public partial RecipeIngredientListModel? IngredientAmountNew { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var recipeResult = (await _mediator.Send(new GetRecipeDetailQuery(Id)));
        if (recipeResult.IsSuccess)
        {
            Recipe = RecipeDetailModel.MapFromResponse(recipeResult.Value);
        }

        Ingredients.Clear();
        
        var filter = new IngredientFilter();
        Result<PagedResult<IngredientListResponse>> ingredientsResult = (await _mediator.Send(new GetIngredientListQuery(filter)));
        if (ingredientsResult.IsSuccess)
        {
            foreach (var ingredient in ingredientsResult.Value.Items)
            {
                Ingredients.Add(IngredientListModel.MapFromResponse(ingredient));
            }
        }
    }

    [RelayCommand]
    private async Task AddNewIngredientToRecipeAsync()
    {
        if (IngredientAmountNew is not null
            && IngredientSelected is not null
            && Id.Value != Guid.Empty
            && IngredientAmountNew.Amount > 0)
        {
            IngredientAmountNew.IngredientId = IngredientSelected.IngredientId;
            IngredientAmountNew.IngredientName = IngredientSelected.IngredientName;
            IngredientAmountNew.IngredientImageUrl = IngredientSelected.IngredientImageUrl;

            var ingredientId = new IngredientId(IngredientAmountNew.IngredientId);
            var request = new RecipeAddIngredientRequest(ingredientId, IngredientAmount.CreateObject(IngredientAmountNew.Amount).Value, IngredientAmountNew.Unit);
            var result = await _mediator.Send(new AddIngredientToRecipeCommand(new RecipeId(Recipe.Id), request));
            if (result.IsSuccess)
            {
                IngredientAmountNew.RecipeIngredientId =  result.Value.Value;
                Recipe.Ingredients.Add(IngredientAmountNew);

                // IngredientAmountNew = GetIngredientAmountNew();

                MessengerService.Send(new RecipeIngredientAddMessage());
            }
        }
    }
    //
    // [RelayCommand]
    // private async Task UpdateIngredientAsync(IngredientInRecipe? model)
    // {
    //     if (Id != Guid.Empty
    //         && model is not null
    //         && model.Amount > 0
    //         && Recipe.Ingredients.Any(i => i.RecipeIngredientId == model.Id))
    //     {
    //         await _mediator.Send(new UpdateIngredientInRecipeCommand(Recipe.Id, model.Id, model.Amount, model.Unit));
    //         MessengerService.Send(new RecipeIngredientEditMessage());
    //     }
    // }
    //
    // [RelayCommand]
    // private async Task RemoveIngredientAsync(RecipeIngredientListModel model)
    // {
    //     if (Recipe. != Guid.Empty)
    //     {
    //         await _mediator.Send(new RemoveIngredientFromRecipeCommand(Recipe.Id, model.Id));
    //         Recipe.Ingredients.Remove(model);
    //
    //         MessengerService.Send(new RecipeIngredientDeleteMessage());
    //     }
    // }

}
