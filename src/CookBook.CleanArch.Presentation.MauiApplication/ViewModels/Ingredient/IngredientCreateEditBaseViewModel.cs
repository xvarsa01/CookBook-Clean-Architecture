using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Validations;
using FluentValidation.Results;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public abstract partial class IngredientCreateEditBaseViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    protected readonly IMediator Mediator = mediator;
    protected readonly INavigationService NavigationService = navigationService;

    private readonly IngredientDetailModelValidator IngredientValidator = new();

    [ObservableProperty]
    public partial IngredientDetailModel Ingredient { get; set; } = IngredientDetailModel.Empty;

    protected async Task<bool> ValidateAsync()
    {
        Ingredient.ValidationResults = await IngredientValidator.ValidateAsync(Ingredient);
        return Ingredient.ValidationResults.IsValid;
    }

    protected ImageUrl? TryCreateImageUrl()
    {
        if (string.IsNullOrEmpty(Ingredient.ImageUrl))
            return null;

        var result = ImageUrl.CreateObject(Ingredient.ImageUrl);
        return result.IsSuccess ? result.Value : null;
    }

    [RelayCommand]
    private async Task ValidateProperty(string propertyName)
    {
        ValidationResult result = await IngredientValidator.ValidateAsync(Ingredient);

        Ingredient.ValidationResults = result;

        Ingredient.ValidationResults.Errors.Remove(
            Ingredient.ValidationResults.Errors.FirstOrDefault(x => x.PropertyName == propertyName));

        Ingredient.ValidationResults.Errors.AddRange(result.Errors);

        OnPropertyChanged(nameof(Ingredient.ValidationResults));
    }
}
