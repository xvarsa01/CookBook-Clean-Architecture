using IngredientListViewModel = CookBook.Clean.App.ViewModels.IngredientListViewModel;

namespace CookBook.Clean.App.Views.Ingredient;

public partial class IngredientListView : ContentPageBase
{
	public IngredientListView(IngredientListViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
