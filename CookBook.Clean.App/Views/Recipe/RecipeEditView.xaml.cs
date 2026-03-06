using RecipeEditViewModel = CookBook.Clean.App.ViewModels.RecipeEditViewModel;

namespace CookBook.Clean.App.Views.Recipe;

public partial class RecipeEditView : ContentPageBase
{
    public RecipeEditView(RecipeEditViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
