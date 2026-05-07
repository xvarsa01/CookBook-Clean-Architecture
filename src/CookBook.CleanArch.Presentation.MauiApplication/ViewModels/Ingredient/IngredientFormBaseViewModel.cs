using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;
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

    [RelayCommand]
    protected async Task<bool> ValidateIngredientAsync()
    {
        Ingredient.ValidationResults = await _ingredientValidator.ValidateAsync(Ingredient);
        
        OnPropertyChanged(nameof(Ingredient.ValidationResults));
        
        return Ingredient.ValidationResults.IsValid;
    }

    protected ImageUrl? TryCreateImageUrl()
    {
        if (string.IsNullOrEmpty(Ingredient.ImageUrl))
            return null;

        var result = ImageUrl.CreateObject(Ingredient.ImageUrl);
        return result.IsSuccess ? result.Value : null;
    }

}
public partial class IngredientFormModel() : ObservableObject
{
    [SetsRequiredMembers]
    public IngredientFormModel(IngredientResponse response) : this()
    {
        Id = response.Id.Value;
        Name = response.Name;
        Description = response.Description;
        ImageUrl = response.ImageUrl?.Value;
    }

    [ObservableProperty]
    public required partial Guid Id { get; set; }
    
    [ObservableProperty]
    public required partial string Name { get; set; }
    [ObservableProperty]
    public partial string? Description { get; set; }
    [ObservableProperty]
    public partial string? ImageUrl { get; set; }

    [ObservableProperty]
    public partial ValidationResult? ValidationResults {get; set; } = new();


    public static IngredientFormModel Empty
        => new()
        {
            Id = Guid.Empty,
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
            .NotNull()
                .WithMessage(_ =>
                {
                    var err = IngredientErrors.IngredientNameEmptyError();
                    var localized = DomainErrorTexts.ResourceManager.GetString(err.Code, System.Globalization.CultureInfo.CurrentUICulture);
                    return string.IsNullOrEmpty(localized) ? err.Message : localized;
                })
            .NotEmpty()
                .WithMessage(_ =>
                {
                    var err = IngredientErrors.IngredientNameEmptyError();
                    var localized = DomainErrorTexts.ResourceManager.GetString(err.Code, System.Globalization.CultureInfo.CurrentUICulture);
                    return string.IsNullOrEmpty(localized) ? err.Message : localized;
                });

        RuleFor(x => x.ImageUrl)
            .IsValidOptionalValueObject<IngredientFormModel, ImageUrl>();
    }
}
