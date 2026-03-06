using RecipeListViewModel = CookBook.Clean.App.ViewModels.RecipeListViewModel;

namespace CookBook.Clean.App.Views.Recipe;

public partial class RecipeListView : ContentPageBase
{
	public RecipeListView(RecipeListViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
