using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Views;

public class IngredientTemplateSelector : DataTemplateSelector
{
    public DataTemplate StandartTemplate { get; set; }
    public DataTemplate EditTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (container?.BindingContext is RecipeEditViewModel vm)
        {
            return vm.IngredientsEditingIsActive
                ? EditTemplate
                : StandartTemplate;
        }

        return StandartTemplate;
    }
}
