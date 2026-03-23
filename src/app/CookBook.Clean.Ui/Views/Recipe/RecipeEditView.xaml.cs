using RecipeEditViewModel = CookBook.Clean.Ui.ViewModels.RecipeEditViewModel;

namespace CookBook.Clean.Ui.Views.Recipe;

public partial class RecipeEditView : ContentPageBase
{
    public RecipeEditView(RecipeEditViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
