using CommunityToolkit.Mvvm.ComponentModel;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain.Recipe.Enums;

namespace CookBook.CleanArch.Presentation.MauiApplication.Models;

public partial class RecipeListModel : ObservableObject
{
    [ObservableProperty]
    public required partial Guid Id { get; set; }
    
    [ObservableProperty]
    public required partial string Name { get; set; }

    // [ObservableProperty]
    // public required partial TimeSpan Duration { get; set; }
    //
    // [ObservableProperty]
    // public partial RecipeType RecipeType { get; set; }

    [ObservableProperty]
    public partial string? ImageUrl { get; set; }

    public static RecipeListModel MapFromResponse(RecipeListResponse response)
    {
        return new RecipeListModel
        {
            Id = response.Id.Value,
            Name = response.Name.Value,
            ImageUrl = response.ImageUrl?.Value
        };
    }
}
