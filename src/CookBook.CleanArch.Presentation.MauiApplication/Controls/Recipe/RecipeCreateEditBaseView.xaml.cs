using System.Collections.ObjectModel;
using System.Windows.Input;
using CookBook.CleanArch.Application.Ingredients.Models;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Models;

namespace CookBook.CleanArch.Presentation.MauiApplication.Controls.Recipe;

public partial class RecipeCreateEditBaseView
{
    public RecipeCreateEditBaseView()
    {
        InitializeComponent();
    }
    
    // ===== MODEL =====
    public static readonly BindableProperty RecipeProperty =
        BindableProperty.Create(nameof(Recipe), typeof(RecipeDetailModel), typeof(RecipeCreateEditBaseView));
    public RecipeDetailModel Recipe
    {
        get => (RecipeDetailModel)GetValue(RecipeProperty);
        set => SetValue(RecipeProperty, value);
    }

    // ===== COMMANDS =====
    public static readonly BindableProperty SaveCommandProperty =
        BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(RecipeCreateEditBaseView));
    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public static readonly BindableProperty ValidatePropertyCommandProperty =
        BindableProperty.Create(nameof(ValidatePropertyCommand), typeof(ICommand), typeof(RecipeCreateEditBaseView));
    public ICommand ValidatePropertyCommand
    {
        get => (ICommand)GetValue(ValidatePropertyCommandProperty);
        set => SetValue(ValidatePropertyCommandProperty, value);
    }

    public static readonly BindableProperty AddIngredientCommandProperty =
        BindableProperty.Create(nameof(AddIngredientCommand), typeof(ICommand), typeof(RecipeCreateEditBaseView));
    public ICommand AddIngredientCommand
    {
        get => (ICommand)GetValue(AddIngredientCommandProperty);
        set => SetValue(AddIngredientCommandProperty, value);
    }

    public static readonly BindableProperty UpdateIngredientCommandProperty =
        BindableProperty.Create(nameof(UpdateIngredientCommand), typeof(ICommand), typeof(RecipeCreateEditBaseView));
    public ICommand UpdateIngredientCommand
    {
        get => (ICommand)GetValue(UpdateIngredientCommandProperty);
        set => SetValue(UpdateIngredientCommandProperty, value);
    }

    public static readonly BindableProperty RemoveIngredientCommandProperty =
        BindableProperty.Create(nameof(RemoveIngredientCommand), typeof(ICommand), typeof(RecipeCreateEditBaseView));
    public ICommand RemoveIngredientCommand
    {
        get => (ICommand)GetValue(RemoveIngredientCommandProperty);
        set => SetValue(RemoveIngredientCommandProperty, value);
    }

    public static readonly BindableProperty ValidateIngredientCommandProperty =
        BindableProperty.Create(nameof(ValidateIngredientCommand), typeof(ICommand), typeof(RecipeCreateEditBaseView));
    public ICommand ValidateIngredientCommand
    {
        get => (ICommand)GetValue(ValidateIngredientCommandProperty);
        set => SetValue(ValidateIngredientCommandProperty, value);
    }

    // ===== DATA =====
    public static readonly BindableProperty IngredientsProperty =
        BindableProperty.Create(nameof(Ingredients), typeof(ObservableCollection<IngredientListResponse>), typeof(RecipeCreateEditBaseView));
    public ObservableCollection<IngredientListResponse> Ingredients
    {
        get => (ObservableCollection<IngredientListResponse>)GetValue(IngredientsProperty);
        set => SetValue(IngredientsProperty, value);
    }
    
    public static readonly BindableProperty UnitsProperty =
        BindableProperty.Create(nameof(Units), typeof(List<MeasurementUnit>), typeof(RecipeCreateEditBaseView));
    public List<MeasurementUnit> Units
    {
        get => (List<MeasurementUnit>)GetValue(UnitsProperty);
        set => SetValue(UnitsProperty, value);
    }

    public static readonly BindableProperty FoodTypesProperty =
        BindableProperty.Create(nameof(FoodTypes), typeof(List<RecipeType>), typeof(RecipeCreateEditBaseView));
    public List<RecipeType> FoodTypes
    {
        get => (List<RecipeType>)GetValue(FoodTypesProperty);
        set => SetValue(FoodTypesProperty, value);
    }
    public static readonly BindableProperty IngredientAmountNewProperty =
        BindableProperty.Create(nameof(IngredientAmountNew), typeof(RecipeIngredientListModel), typeof(RecipeCreateEditBaseView));
    public RecipeIngredientListModel IngredientAmountNew
    {
        get => (RecipeIngredientListModel)GetValue(IngredientAmountNewProperty);
        set => SetValue(IngredientAmountNewProperty, value);
    }

    public static readonly BindableProperty SelectedNewIngredientProperty =
        BindableProperty.Create(nameof(SelectedNewIngredient), typeof(IngredientListResponse), typeof(RecipeCreateEditBaseView), null, BindingMode.TwoWay);
    public IngredientListResponse? SelectedNewIngredient
    {
        get => (IngredientListResponse?)GetValue(SelectedNewIngredientProperty);
        set => SetValue(SelectedNewIngredientProperty, value);
    }
}

