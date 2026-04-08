using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Commands.Ingredients;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Application.Queries.Ingredients;
using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Validations;
using FluentValidation.Results;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class IngredientEditViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    private readonly IngredientDetailModelValidator _ingredientValidator = new();
    
    public IngredientId Id { get; set; } = new(Guid.Empty);

    [ObservableProperty]
    public partial IngredientDetailModel Ingredient { get; set; } = IngredientDetailModel.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        if (Id.Value == Guid.Empty)
        {
            return;
        }

        var result = (await mediator.Send(new GetIngredientDetailQuery(Id)));
        if (result.IsSuccess)
        {
            Ingredient = new IngredientDetailModel(result.Value);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var validator = new IngredientDetailModelValidator();
        Ingredient.ValidationResults = await validator.ValidateAsync(Ingredient);
        if (!Ingredient.ValidationResults.IsValid)
        {
            return;
        }
        
        ImageUrl? imageUrl = null;
        if (!string.IsNullOrEmpty(Ingredient.ImageUrl))
        {
            var result = ImageUrl.CreateObject(Ingredient.ImageUrl);
            if (result.IsFailure)
            {
                return;     // This should never happen if validation ran, but just to be sure, we check it again here before saving.
            }
            
            imageUrl = result.Value;
        };
        
        if (Id.Value == Guid.Empty)
        {
            var createRequest = new IngredientCreateRequest(Ingredient.Name, Ingredient.Description, imageUrl);
            await mediator.Send(new CreateIngredientCommand(createRequest));
        }
        else
        {
            var updateRequest = new IngredientUpdateRequest(Id, Ingredient.Name, Ingredient.Description, imageUrl);
            await mediator.Send(new UpdateIngredientCommand(updateRequest));
        }

        MessengerService.Send(new IngredientEditMessage { IngredientId = Id });

        navigationService.SendBackButtonPressed();
    }
    
    [RelayCommand]
    private async Task ValidateProperty(string propertyName)
    {
        ValidationResult result = await _ingredientValidator.ValidateAsync(Ingredient);
        Ingredient.ValidationResults = result;
        
        // Remove the previous error message for this property
        Ingredient.ValidationResults.Errors.Remove(Ingredient.ValidationResults.Errors.FirstOrDefault(x => x.PropertyName == propertyName));
        Ingredient.ValidationResults.Errors.AddRange(result.Errors);

        // Notify the UI that the property has changed
        OnPropertyChanged(nameof(Ingredient.ValidationResults));
    }

}
