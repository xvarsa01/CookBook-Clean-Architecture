using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views.Recipe;

public partial class RecipeCreateView
{
    public RecipeCreateView(RecipeCreateViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}

