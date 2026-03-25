using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views.Recipe;

public partial class RecipeIngredientsEditView : ContentPageBase
{
    public RecipeIngredientsEditView(RecipeIngredientsEditViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
