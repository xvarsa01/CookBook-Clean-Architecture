using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain.Recipes.Enums;
using FluentValidation.Results;

namespace CookBook.CleanArch.Presentation.MauiApplication.Models;

public partial class RecipeDetailModel : ObservableObject
{
    public RecipeDetailModel() { }

    [SetsRequiredMembers]
    public RecipeDetailModel(RecipeResponse response)
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
    
    public static RecipeDetailModel Empty
        => new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Duration = TimeSpan.Zero
        };
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
            Unit = MeasurementUnit.Unit
        };
}
