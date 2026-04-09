using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views.Ingredient;

public partial class IngredientListView : ContentPageBase
{
	public IngredientListView(IngredientListViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
