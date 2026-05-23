using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Ingredients;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Application.Ingredients.Queries;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Shared;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Extensions;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.Validations;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public abstract partial class RecipeFormBaseViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
        IRecipient<RecipeIngredientEditMessage>,
        IRecipient<RecipeIngredientAddMessage>,
        IRecipient<RecipeIngredientDeleteMessage>
{
    protected readonly IMediator Mediator = mediator;
    protected readonly INavigationService NavigationService = navigationService;

    private readonly RecipeFormModel.Validator _recipeValidator = new();
    private readonly RecipeIngredientListModel.Validator _ingredientValidator = new();

    [ObservableProperty]
    public partial RecipeFormModel Recipe { get; set; } = RecipeFormModel.Empty;

    public List<RecipeType> FoodTypes { get; } = [.. Enum.GetValues<RecipeType>()];
    public List<MeasurementUnit> Units { get; } = [.. Enum.GetValues<MeasurementUnit>()];

    [ObservableProperty]
    public partial RecipeIngredientListModel IngredientAmountNew { get; set; } = RecipeIngredientListModel.Empty;

    [ObservableProperty]
    public partial ObservableCollection<IngredientListResponse> Ingredients { get; set; } = [];

    public IngredientListResponse? SelectedNewIngredient
    {
        get;
        set
        {
            if (!SetProperty(ref field, value) || value is null)
                return;

            IngredientAmountNew.IngredientId = value.Id.Value;
            IngredientAmountNew.IngredientName = value.Name;
            IngredientAmountNew.IngredientImageUrl = value.ImageUrl?.Value;
        }
    }

    protected async Task LoadIngredientsAsync()
    {
        var paging = new PagingOptions { PageSize = 1000 };
        var result = await Mediator.Send(new GetIngredientListQuery(new IngredientFilter(),  paging));
        if (!result.IsSuccess)
            return;

        Ingredients.Clear();
        foreach (var item in result.Value.Items)
            Ingredients.Add(item);
    }

    [RelayCommand]
    protected async Task<bool> ValidateRecipeAsync()
    {
        Recipe.ValidationResults = await _recipeValidator.ValidateAsync(Recipe);
        
        OnPropertyChanged(nameof(Recipe.ValidationResults));
        
        return Recipe.ValidationResults.IsValid;
    }
    
    protected async Task<bool> ValidateExistingIngredientAsync(RecipeIngredientListModel model)
    {
        model.ValidationResults = await _ingredientValidator.ValidateAsync(model);
        Recipe.ValidationResults = await _recipeValidator.ValidateAsync(
            Recipe, options => options.IncludeProperties(nameof(Recipe.Ingredients)));
        
        OnPropertyChanged(nameof(model.ValidationResults));
        OnPropertyChanged(nameof(Recipe.ValidationResults));
        
        return model.ValidationResults.IsValid;
    }

    [RelayCommand]
    protected async Task<bool> ValidateNewIngredientAsync()
    {
        IngredientAmountNew.ValidationResults = await _ingredientValidator.ValidateAsync(IngredientAmountNew);
        
        OnPropertyChanged(nameof(IngredientAmountNew.ValidationResults));
        
        return IngredientAmountNew.ValidationResults.IsValid;
    }

    protected ImageUrl? TryCreateImageUrl()
    {
        if (string.IsNullOrEmpty(Recipe.ImageUrl))
            return null;

        var result = ImageUrl.CreateObject(Recipe.ImageUrl);
        return result.IsSuccess ? result.Value : null;
    }

    public void Receive(RecipeIngredientEditMessage message) => ForceDataRefreshOnNextAppearing();
    public void Receive(RecipeIngredientAddMessage message) => ForceDataRefreshOnNextAppearing();
    public void Receive(RecipeIngredientDeleteMessage message) => ForceDataRefreshOnNextAppearing();
}

public partial class RecipeFormModel : ObservableObject
{
    public RecipeFormModel() { }

    [SetsRequiredMembers]
    public RecipeFormModel(RecipeResponse response)
    {
        Id = response.Id.Value;
        Name = response.Name.Value;
        Description = response.Description;
        Duration = response.Duration.Value;
        RecipeType = response.Type;
        ImageUrl = response.ImageUrl?.Value;
        Ingredients = response.Ingredients
            .Select(i => new RecipeIngredientListModel
            {
                RecipeIngredientId = i.Id.Value,
                IngredientId = i.IngredientId.Value,
                IngredientName = i.IngredientName,
                IngredientImageUrl = i.IngredientImageUrl?.Value,
                Amount = i.Amount.Value,
                Unit = i.Unit
            })
            .ToObservableCollection();
    }

    [ObservableProperty]
    public required partial Guid Id { get; set; }
    
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public required partial string? Description { get; set; }

    [ObservableProperty]
    public required partial TimeSpan Duration { get; set; }

    [ObservableProperty]
    public partial RecipeType RecipeType { get; set; }

    [ObservableProperty]
    public partial string? ImageUrl { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<RecipeIngredientListModel> Ingredients { get; set; } = [];
    
    [ObservableProperty]
    public partial ValidationResult? ValidationResults {get; set; } = new();
    
    public static RecipeFormModel Empty
        => new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Duration = TimeSpan.Zero
        };
    
    public static string RecipeNameProperty => nameof(Name);
    public static string RecipeDurationProperty => nameof(Duration);
    public static string RecipeImageUrlProperty => nameof(ImageUrl);
    public static string RecipeRecipeTypeProperty => nameof(RecipeType);
    
    public class Validator : AbstractValidator<RecipeFormModel>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .IsValidValueObject<RecipeFormModel, RecipeName>();

            RuleFor(x => x.Duration)
                .IsValidValueObject<RecipeFormModel, RecipeDuration>();

            RuleFor(x => x.RecipeType)
                .NotEqual(RecipeType.None)
                .WithMessage(_ => RecipeErrors.RecipeTypeNotSelectedError().ToLocalizedMessage());

            RuleFor(x => x.ImageUrl)
                .IsValidOptionalValueObject<RecipeFormModel, ImageUrl>();
            
            RuleFor(x => x.Ingredients)
                .Must(i => i.Count > 0)
                .WithMessage(_ => RecipeErrors.RecipeMinimumNumberOfIngredientsError().ToLocalizedMessage());

            RuleFor(x => x.Ingredients)
                .Must(i => i.Count <= 10)
                .WithMessage(_ => RecipeErrors.RecipeMaximumNumberOfIngredientsError().ToLocalizedMessage());
            
            RuleForEach(x => x.Ingredients)
                .SetValidator(new RecipeIngredientListModel.Validator());
        }
    }

}

public partial class RecipeIngredientListModel : ObservableObject
{
    [ObservableProperty]
    public required partial Guid RecipeIngredientId { get; set; }
    
    [ObservableProperty]
    public required partial Guid IngredientId { get; set; }

    [ObservableProperty]
    public required partial string IngredientName { get; set; }

    [ObservableProperty]
    public required partial string? IngredientImageUrl { get; set; }

    [ObservableProperty]
    public required partial decimal Amount { get; set; }

    [ObservableProperty]
    public required partial MeasurementUnit Unit { get; set; }
    
    [ObservableProperty]
    public partial ValidationResult? ValidationResults {get; set; } = new();
    
    public static RecipeIngredientListModel Empty
        => new()
        {
            RecipeIngredientId = Guid.Empty,
            IngredientId = Guid.Empty,
            IngredientName = string.Empty,
            IngredientImageUrl = string.Empty,
            Amount = 0,
            Unit = MeasurementUnit.None
        };

    public static string IngredientIdProperty => nameof(IngredientId);
    public static string IngredientAmountProperty => nameof(Amount);
    public static string IngredientUnitProperty => nameof(Unit);
    
    public class Validator : AbstractValidator<RecipeIngredientListModel>
    {
        public Validator()
        {
            RuleFor(x => x.IngredientId)
                .NotEqual(Guid.Empty)
                .WithMessage(_ => RecipeIngredientErrors.IngredientNotSelectedError().ToLocalizedMessage());
        
            RuleFor(x => x.Amount)
                .IsValidValueObject<RecipeIngredientListModel, IngredientAmount>();
        
            RuleFor(x => x.Unit)
                .NotEqual(MeasurementUnit.None)
                .WithMessage(_ => RecipeIngredientErrors.UnitNotSelectedError().ToLocalizedMessage());
        }
    }
}
