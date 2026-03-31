using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Presentation.MauiApplication.Models;

public partial class RecipeDetailModel : ObservableObject
{
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
    
    public static RecipeDetailModel MapFromResponse(RecipeResponse response)
    {
        return new RecipeDetailModel
        {
            Id = response.Id.Value,
            Name = response.Name.Value,
            Description = response.Description,
            Duration = response.Duration.Value,
            RecipeType = response.Type,
            ImageUrl = response.ImageUrl?.Value
        };
    }
    
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
}
