using IngredientListViewModel = CookBook.Clean.Ui.ViewModels.IngredientListViewModel;

namespace CookBook.Clean.Ui.Views.Ingredient;

public partial class IngredientListView : ContentPageBase
{
	public IngredientListView(IngredientListViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
