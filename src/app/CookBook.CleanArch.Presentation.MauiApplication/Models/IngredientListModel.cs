using CommunityToolkit.Mvvm.ComponentModel;
using CookBook.CleanArch.Application.Models.Ingredient;

namespace CookBook.CleanArch.Presentation.MauiApplication.Models;

public partial class IngredientListModel : ObservableObject
{
    [ObservableProperty]
    public required partial Guid Id { get; set; }
    
    [ObservableProperty]
    public required partial string Name { get; set; }
    
    [ObservableProperty]
    public partial string? ImageUrl { get; set; }
    
    public static IngredientListModel MapFromResponse(IngredientListResponse response)
    {
        return new IngredientListModel
        {
            Id = response.Id.Value,
            Name = response.Name,
            ImageUrl = response.ImageUrl?.Value
        };
    }
}
