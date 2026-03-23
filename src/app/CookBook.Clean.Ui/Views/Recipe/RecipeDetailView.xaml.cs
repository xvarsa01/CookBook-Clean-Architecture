using RecipeDetailViewModel = CookBook.Clean.Ui.ViewModels.RecipeDetailViewModel;

namespace CookBook.Clean.Ui.Views.Recipe;

public partial class RecipeDetailView : ContentPageBase
{
	public RecipeDetailView(RecipeDetailViewModel viewModel)
        : base(viewModel)
    {
		InitializeComponent();
	}
}
