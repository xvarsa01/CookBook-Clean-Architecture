using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;
using FluentValidation.TestHelper;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Recipes;

public class RecipeFormValidationTests : MauiTestsBase
{
    private readonly RecipeFormModelValidator _recipeValidator = new();
    private readonly RecipeIngredientListModelValidator _ingredientValidator = new();

    [Fact]
    public void Should_Have_Error_When_Recipe_Name_Is_Empty()
    {
        var model = RecipeFormModel.Empty;
        model.Name = string.Empty;
        model.Duration = TimeSpan.FromMinutes(10);
        model.RecipeType = RecipeType.MainDish;
        model.Ingredients.Add(new RecipeIngredientListModel
        {
            RecipeIngredientId = Guid.Empty,
            IngredientId = Guid.NewGuid(),
            IngredientName = "salt",
            IngredientImageUrl = null,
            Amount = 1,
            Unit = MeasurementUnit.Pieces
        });

        var result = _recipeValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Recipe_Is_Missing_Ingredients()
    {
        var model = RecipeFormModel.Empty;
        model.Name = "valid name";
        model.Duration = TimeSpan.FromMinutes(10);
        model.RecipeType = RecipeType.MainDish;

        var result = _recipeValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Ingredients);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Recipe_Is_Valid()
    {
        var model = RecipeFormModel.Empty;
        model.Name = "valid name";
        model.Description = "desc";
        model.Duration = TimeSpan.FromMinutes(10);
        model.RecipeType = RecipeType.MainDish;
        model.Ingredients.Add(new RecipeIngredientListModel
        {
            RecipeIngredientId = Guid.Empty,
            IngredientId = Guid.NewGuid(),
            IngredientName = "salt",
            IngredientImageUrl = null,
            Amount = 1,
            Unit = MeasurementUnit.Pieces
        });

        var result = _recipeValidator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Ingredients);
    }

    [Fact]
    public void Should_Have_Error_When_Ingredient_Id_Is_Empty()
    {
        var model = RecipeIngredientListModel.Empty;
        model.Amount = 1;
        model.Unit = MeasurementUnit.Pieces;

        var result = _ingredientValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.IngredientId);
    }

    [Fact]
    public void Should_Have_Error_When_Ingredient_Amount_Is_Not_Positive()
    {
        var model = RecipeIngredientListModel.Empty;
        model.IngredientId = Guid.NewGuid();
        model.Unit = MeasurementUnit.Pieces;

        var result = _ingredientValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }
}
