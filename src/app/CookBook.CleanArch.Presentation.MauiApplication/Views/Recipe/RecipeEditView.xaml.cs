using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views.Recipe;

public partial class RecipeEditView : ContentPageBase
{
    public RecipeEditView(RecipeEditViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
