using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Commands.Recipes;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeIngredientsEditViewModel(
    IMediator _mediator,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }

    public List<MeasurementUnit> Units { get; set; } = [.. Enum.GetValues<MeasurementUnit>()];

    [ObservableProperty]
    public partial RecipeDetailModel Recipe { get; set; } = RecipeDetailModel.Empty;

    [ObservableProperty]
    public partial ObservableCollection<IngredientListModel> Ingredients { get; set; } = [];

    [ObservableProperty]
    public partial IngredientListModel? IngredientSelected { get; set; }

    [ObservableProperty]
    public partial IngredientInRecipe? IngredientAmountNew { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var recipeResult = (await _mediator.Send(new GetRecipeDetailQuery(Id)));
        if (recipeResult.IsSuccess && recipeResult.Value is not null)
        {
            Recipe = recipeResult.Value;
        }

        Ingredients.Clear();
        
        var filter = new IngredientFilter();
        var ingredientsResult = (await _mediator.Send(new GetIngredientListQuery(filter)));
        if (ingredientsResult.IsSuccess && ingredientsResult.Value is not null)
        {
            foreach (var ingredient in ingredientsResult.Value)
            {
                Ingredients.Add(ingredient);
                IngredientAmountNew = GetIngredientAmountNew();
            }
        }
    }

    [RelayCommand]
    private async Task AddNewIngredientToRecipeAsync()
    {
        if (IngredientAmountNew is not null
            && IngredientSelected is not null
            && Recipe.Id != Guid.Empty
            && IngredientAmountNew.Amount > 0)
        {
            IngredientAmountNew.IngredientId = IngredientSelected.Id;
            IngredientAmountNew.Name = IngredientSelected.Name;
            IngredientAmountNew.ImageUrl = IngredientSelected.ImageUrl;

            var result = await _mediator.Send(new AddIngredientToRecipeCommand(Recipe.Id, IngredientAmountNew.IngredientId, IngredientAmountNew.Amount, IngredientAmountNew.Unit));
            if (result.IsSuccess)
            {
                IngredientAmountNew.Id =  result.Value;
                Recipe.Ingredients.Add(IngredientAmountNew);

                IngredientAmountNew = GetIngredientAmountNew();

                MessengerService.Send(new RecipeIngredientAddMessage());
            }
        }
    }

    [RelayCommand]
    private async Task UpdateIngredientAsync(IngredientInRecipe? model)
    {
        if (Recipe.Id != Guid.Empty
            && model is not null
            && model.Amount > 0
            && Recipe.Ingredients.Any(i => i.Id == model.Id))
        {
            await _mediator.Send(new UpdateIngredientInRecipeCommand(Recipe.Id, model.Id, model.Amount, model.Unit));
            MessengerService.Send(new RecipeIngredientEditMessage());
        }
    }

    [RelayCommand]
    private async Task RemoveIngredientAsync(IngredientInRecipe model)
    {
        if (Recipe.Id != Guid.Empty)
        {
            await _mediator.Send(new RemoveIngredientFromRecipeCommand(Recipe.Id, model.Id));
            Recipe.Ingredients.Remove(model);

            MessengerService.Send(new RecipeIngredientDeleteMessage());
        }
    }

    private IngredientInRecipe GetIngredientAmountNew()
    {
        var ingredientFirst = Ingredients.First();
        return new()
        {
            Id = Guid.Empty,
            IngredientId = ingredientFirst.Id,
            Name = ingredientFirst.Name,
            ImageUrl = ingredientFirst.ImageUrl,
            Amount = 0,
            Unit = MeasurementUnit.Unit,
        };
    }
}
