using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Commands.Recipes;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Validations;
using FluentValidation.Results;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RecipeEditViewModel(
    IMediator _mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<RecipeIngredientEditMessage>, IRecipient<RecipeIngredientAddMessage>,
        IRecipient<RecipeIngredientDeleteMessage>
{
    public RecipeId Id { get; set; }

    [ObservableProperty]
    public partial RecipeDetailModel Recipe { get; set; } = RecipeDetailModel.Empty;

    public List<RecipeType> FoodTypes { get; set; } = [.. Enum.GetValues<RecipeType>()];

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await _mediator.Send(new GetRecipeDetailQuery(Id)));
        if (result.IsSuccess)
        {
            Recipe = RecipeDetailModel.MapFromResponse(result.Value);
        }
    }

    [RelayCommand]
    private async Task GoToRecipeIngredientEditAsync()
    {
        await navigationService.GoToAsync(NavigationService.RecipeIngredientsEditRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(RecipeIngredientsEditViewModel.Id)] = Id
                });
    }

    [RelayCommand]
    private async Task SaveAsync()
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
            await _mediator.Send(new CreateRecipeCommand(request));
        }
        else
        {
            var request = new RecipeUpdateRequest(new RecipeId(Recipe.Id), RecipeName.CreateObject(Recipe.Name).Value, Recipe.Description, imageUrl, RecipeDuration.CreateObject(Recipe.Duration).Value, Recipe.RecipeType);
            await _mediator.Send(new UpdateRecipeCommand(request));
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
