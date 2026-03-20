using RecipeIngredientsEditViewModel = CookBook.Clean.Ui.ViewModels.RecipeIngredientsEditViewModel;

namespace CookBook.Clean.Ui.Views.Recipe;

public partial class RecipeIngredientsEditView : ContentPageBase
{
    public RecipeIngredientsEditView(RecipeIngredientsEditViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
