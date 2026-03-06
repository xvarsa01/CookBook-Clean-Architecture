using RecipeDetailViewModel = CookBook.Clean.App.ViewModels.RecipeDetailViewModel;

namespace CookBook.Clean.App.Views.Recipe;

public partial class RecipeDetailView : ContentPageBase
{
	public RecipeDetailView(RecipeDetailViewModel viewModel)
        : base(viewModel)
    {
		InitializeComponent();
	}
}
