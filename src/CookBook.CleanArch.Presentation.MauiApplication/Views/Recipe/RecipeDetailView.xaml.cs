using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views.Recipe;

public partial class RecipeDetailView : ContentPageBase
{
	public RecipeDetailView(RecipeDetailViewModel viewModel)
        : base(viewModel)
    {
		InitializeComponent();
	}
}
