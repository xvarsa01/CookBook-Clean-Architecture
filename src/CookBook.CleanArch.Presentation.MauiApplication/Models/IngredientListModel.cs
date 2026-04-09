using CommunityToolkit.Mvvm.ComponentModel;

namespace CookBook.CleanArch.Presentation.MauiApplication.Models;

public partial class IngredientListModel : ObservableObject
{
    [ObservableProperty]
    public required partial Guid Id { get; set; }
    
    [ObservableProperty]
    public required partial string Name { get; set; }
    
    [ObservableProperty]
    public partial string? ImageUrl { get; set; }
}
