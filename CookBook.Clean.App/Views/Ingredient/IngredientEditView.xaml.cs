using IngredientEditViewModel = CookBook.Clean.App.ViewModels.IngredientEditViewModel;

namespace CookBook.Clean.App.Views.Ingredient;

public partial class IngredientEditView : ContentPageBase
{
	public IngredientEditView(IngredientEditViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
