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

    // ===== COMMANDS =====
    public static readonly BindableProperty SaveCommandProperty =
        BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(IngredientFormBaseView));
    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public static readonly BindableProperty CancelChangesCommandProperty =
        BindableProperty.Create(nameof(CancelChangesCommand), typeof(ICommand), typeof(IngredientFormBaseView));
    public ICommand CancelChangesCommand
    {
        get => (ICommand)GetValue(CancelChangesCommandProperty);
        set => SetValue(CancelChangesCommandProperty, value);
    }

    public static readonly BindableProperty ValidateIngredientCommandProperty =
        BindableProperty.Create(nameof(ValidateIngredientCommand), typeof(ICommand), typeof(IngredientFormBaseView));
    public ICommand ValidateIngredientCommand
    {
        get => (ICommand)GetValue(ValidateIngredientCommandProperty);
        set => SetValue(ValidateIngredientCommandProperty, value);
    }
}

