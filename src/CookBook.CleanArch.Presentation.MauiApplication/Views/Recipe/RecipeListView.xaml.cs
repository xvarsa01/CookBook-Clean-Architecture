using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views.Recipe;

public partial class RecipeListView : ContentPageBase
{
	public RecipeListView(RecipeListViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
