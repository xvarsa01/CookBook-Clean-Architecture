using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Validations;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public abstract partial class IngredientFormBaseViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    protected readonly IMediator Mediator = mediator;
    protected readonly INavigationService NavigationService = navigationService;

    private readonly IngredientFormModelValidator _ingredientValidator = new();

    [ObservableProperty]
    public partial IngredientFormModel Ingredient { get; set; } = IngredientFormModel.Empty;

    protected async Task<bool> ValidateAsync()
    {
        Ingredient.ValidationResults = await _ingredientValidator.ValidateAsync(Ingredient);
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
        ValidationResult result = await _ingredientValidator.ValidateAsync(Ingredient);

        Ingredient.ValidationResults = result;

        OnPropertyChanged(nameof(Ingredient.ValidationResults));
    }

}
public partial class IngredientFormModel() : ObservableObject
{
    public IngredientFormModel(IngredientResponse response) : this()
    {
        Name = response.Name;
        Description = response.Description;
        ImageUrl = response.ImageUrl?.Value;
    }

    [ObservableProperty]
    public partial string Name { get; set; }
    [ObservableProperty]
    public partial string? Description { get; set; }
    [ObservableProperty]
    public partial string? ImageUrl { get; set; }

    [ObservableProperty]
    public partial ValidationResult? ValidationResults {get; set; } = new();


    public static IngredientFormModel Empty
        => new()
        {
            Name = string.Empty,
            Description = string.Empty,
            ImageUrl = null
        };
}

public class IngredientFormModelValidator : AbstractValidator<IngredientFormModel>
{
    public static string IngredientNameProperty => nameof(IngredientFormModel.Name);
    public static string IngredientImageUrlProperty => nameof(IngredientFormModel.ImageUrl);

    public IngredientFormModelValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("The ingredient name must not be empty")
            .NotEmpty().WithMessage("The ingredient name must not be empty");

        RuleFor(x => x.ImageUrl)
            .IsValidOptionalValueObject<IngredientFormModel, ImageUrl>();
    }
}
