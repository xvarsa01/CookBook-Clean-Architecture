using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.Clean.App.Messages;
using CookBook.Clean.App.Services.Interfaces;
using CookBook.Clean.Core.Recipe;
using CookBook.Clean.UseCases.Ingredient;
using CookBook.Clean.UseCases.Ingredient.GetList;
using CookBook.Clean.UseCases.Recipe;
using CookBook.Clean.UseCases.Recipe.AddIngredient;
using CookBook.Clean.UseCases.Recipe.Get;
using CookBook.Clean.UseCases.Recipe.IngredientUpdate;
using CookBook.Clean.UseCases.Recipe.RemoveIngredient;
using MediatR;

namespace CookBook.Clean.App.ViewModels;

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
    public partial IngredientInRecipeModel? IngredientAmountNew { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var recipeResult = (await _mediator.Send(new GetRecipeUseCase(Id)));
        if (recipeResult.Success && recipeResult.Value is not null)
        {
            Recipe = recipeResult.Value.Recipe;
        }

        Ingredients.Clear();
        var ingredientsResult = (await _mediator.Send(new GetListIngredientUseCase()));
        if (ingredientsResult.Success && ingredientsResult.Value is not null)
        {
            foreach (var ingredient in ingredientsResult.Value.Ingredients)
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
            && Recipe.Id != Guid.Empty)
        {
            IngredientAmountNew.IngredientId = IngredientSelected.Id;
            IngredientAmountNew.Name = IngredientSelected.Name;
            IngredientAmountNew.ImageUrl = IngredientSelected.ImageUrl;

            await _mediator.Send(new AddIngredientToRecipeUseCase(Recipe.Id, IngredientAmountNew.IngredientId, IngredientAmountNew.Amount, IngredientAmountNew.Unit));
            Recipe.Ingredients.Add(IngredientAmountNew);

            IngredientAmountNew = GetIngredientAmountNew();

            MessengerService.Send(new RecipeIngredientAddMessage());
        }
    }

    [RelayCommand]
    private async Task UpdateIngredientAsync(object? model)
    {
        // if (Recipe.Id != Guid.Empty
        //     && model is not null
        //     && model.Amount > 0)
        // {
        //     await _mediator.Send(new UpdateIngredientInRecipeUseCase(Recipe.Id, model.Id,  model.Amount, model.Unit));
        //     MessengerService.Send(new RecipeIngredientEditMessage());
        // }
    }

    [RelayCommand]
    private async Task RemoveIngredientAsync(IngredientInRecipeModel model)
    {
        if (Recipe.Id != Guid.Empty)
        {
            await _mediator.Send(new RemoveIngredientFromRecipeUseCase(Recipe.Id, model.Id));
            Recipe.Ingredients.Remove(model);

            MessengerService.Send(new RecipeIngredientDeleteMessage());
        }
    }

    private IngredientInRecipeModel GetIngredientAmountNew()
    {
        var ingredientFirst = Ingredients.First();
        return new()
        {
            Id = Guid.NewGuid(),
            IngredientId = ingredientFirst.Id,
            Name = ingredientFirst.Name,
            ImageUrl = ingredientFirst.ImageUrl,
            Amount = 0,
            Unit = MeasurementUnit.Unit,
        };
    }
}
