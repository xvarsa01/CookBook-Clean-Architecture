using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CookBook.CleanArch.Presentation.MauiApplication.Models;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls;

public partial class IngredientCreateEditBaseView
{
    public IngredientCreateEditBaseView()
    {
        InitializeComponent();
    }
    
    // INGREDIENT MODEL
    public static readonly BindableProperty IngredientProperty =
        BindableProperty.Create(nameof(Ingredient), typeof(IngredientDetailModel), typeof(IngredientCreateEditBaseView), default(IngredientDetailModel), BindingMode.TwoWay);
    public IngredientDetailModel Ingredient
    {
        get => (IngredientDetailModel)GetValue(IngredientProperty);
        set => SetValue(IngredientProperty, value);
    }

    // SAVE COMMAND
    public static readonly BindableProperty SaveCommandProperty =
        BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(IngredientCreateEditBaseView));
    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    // VALIDATE PROPERTY COMMAND
    public static readonly BindableProperty ValidatePropertyCommandProperty =
        BindableProperty.Create(nameof(ValidatePropertyCommand), typeof(ICommand), typeof(IngredientCreateEditBaseView));
    public ICommand ValidatePropertyCommand
    {
        get => (ICommand)GetValue(ValidatePropertyCommandProperty);
        set => SetValue(ValidatePropertyCommandProperty, value);
    }
}

