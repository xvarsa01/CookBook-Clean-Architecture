using RecipeIngredientsEditViewModel = CookBook.Clean.App.ViewModels.RecipeIngredientsEditViewModel;

namespace CookBook.Clean.App.Views.Recipe;

public partial class RecipeIngredientsEditView : ContentPageBase
{
    public RecipeIngredientsEditView(RecipeIngredientsEditViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
