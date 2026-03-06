using IngredientDetailViewModel = CookBook.Clean.App.ViewModels.IngredientDetailViewModel;

namespace CookBook.Clean.App.Views.Ingredient;

public partial class IngredientDetailView : ContentPageBase
{
	public IngredientDetailView(IngredientDetailViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
