using IngredientEditViewModel = CookBook.Clean.Ui.ViewModels.IngredientEditViewModel;

namespace CookBook.Clean.Ui.Views.Ingredient;

public partial class IngredientEditView : ContentPageBase
{
	public IngredientEditView(IngredientEditViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
