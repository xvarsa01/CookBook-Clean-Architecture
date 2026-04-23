using System.Windows.Input;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Ingredients;

public partial class IngredientFormBaseView
{
    public IngredientFormBaseView()
    {
        InitializeComponent();
    }
    
    // INGREDIENT MODEL
    public static readonly BindableProperty IngredientProperty =
        BindableProperty.Create(nameof(Ingredient), typeof(IngredientFormModel), typeof(IngredientFormBaseView), default(IngredientFormModel), BindingMode.TwoWay);
    public IngredientFormModel Ingredient
    {
        get => (IngredientFormModel)GetValue(IngredientProperty);
        set => SetValue(IngredientProperty, value);
    }

    // SAVE COMMAND
    public static readonly BindableProperty SaveCommandProperty =
        BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(IngredientFormBaseView));
    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    // VALIDATE PROPERTY COMMAND
    public static readonly BindableProperty ValidatePropertyCommandProperty =
        BindableProperty.Create(nameof(ValidatePropertyCommand), typeof(ICommand), typeof(IngredientFormBaseView));
    public ICommand ValidatePropertyCommand
    {
        get => (ICommand)GetValue(ValidatePropertyCommandProperty);
        set => SetValue(ValidatePropertyCommandProperty, value);
    }
}

