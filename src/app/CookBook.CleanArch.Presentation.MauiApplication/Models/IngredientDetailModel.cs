using CommunityToolkit.Mvvm.ComponentModel;
using CookBook.CleanArch.Application.Models.Ingredient;
using FluentValidation.Results;

namespace CookBook.CleanArch.Presentation.MauiApplication.Models;

public partial class IngredientDetailModel : ObservableObject
{
    [ObservableProperty]
    public required partial string Name { get; set; }
    [ObservableProperty]
    public partial string? Description { get; set; }
    [ObservableProperty]
    public partial string? ImageUrl { get; set; }
    
    [ObservableProperty]
    public partial ValidationResult? ValidationResults {get; set; } = new();
    
    public static IngredientDetailModel MapFromResponse(IngredientDetailResponse response)
    {
        return new IngredientDetailModel
        {
            Name = response.Name,
            Description = response.Description,
            ImageUrl = response.ImageUrl?.Value
        };
    }
    
    public static IngredientDetailModel Empty
        => new()
        {
            Name = string.Empty,
            Description = string.Empty,
            ImageUrl = null
        };
}
