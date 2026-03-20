using IngredientDetailViewModel = CookBook.Clean.Ui.ViewModels.IngredientDetailViewModel;

namespace CookBook.Clean.Ui.Views.Ingredient;

public partial class IngredientDetailView : ContentPageBase
{
	public IngredientDetailView(IngredientDetailViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
