using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views.Ingredient;

public partial class IngredientEditView : ContentPageBase
{
    public IngredientEditView(IngredientEditViewModel viewModel)
        : base(viewModel)
    {
        base.InitializeComponent();
    }
}
