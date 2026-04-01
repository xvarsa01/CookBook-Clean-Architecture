using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CookBook.CleanArch.Application.Models.Ingredient;
using CookBook.CleanArch.Presentation.MauiApplication.Validations;

namespace CookBook.CleanArch.Presentation.MauiApplication.Models;

public partial class IngredientDetailModel : ObservableObject
{
    [ObservableProperty]
    public required partial ValidatableObject<string> Name { get; set; }
    [ObservableProperty]
    public partial string? Description { get; set; }
    [ObservableProperty]
    public partial string? ImageUrl { get; set; }
    
    public static IngredientDetailModel MapFromResponse(IngredientDetailResponse response)
    {
        return new IngredientDetailModel
        {
            Name = new ValidatableObject<string> { Value = response.Name },
            Description = response.Description,
            ImageUrl = response.ImageUrl?.Value
        };
    }
    
    public static IngredientDetailModel Empty
        => new()
        {
            Name = new ValidatableObject<string> { Value = string.Empty },
            Description = string.Empty,
            ImageUrl = null
        };
}
