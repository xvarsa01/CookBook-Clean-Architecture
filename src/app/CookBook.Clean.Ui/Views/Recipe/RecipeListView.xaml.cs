using RecipeListViewModel = CookBook.Clean.Ui.ViewModels.RecipeListViewModel;

namespace CookBook.Clean.Ui.Views.Recipe;

public partial class RecipeListView : ContentPageBase
{
	public RecipeListView(RecipeListViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
