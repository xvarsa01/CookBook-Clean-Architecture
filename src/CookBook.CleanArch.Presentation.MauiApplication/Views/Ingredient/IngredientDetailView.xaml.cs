using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views.Ingredient;

public partial class IngredientDetailView : ContentPageBase
{
	public IngredientDetailView(IngredientDetailViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}
