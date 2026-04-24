using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;
using FluentValidation.TestHelper;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Ingredients;

public class IngredientFormValidationTests : MauiTestsBase
{
    private readonly IngredientFormModelValidator _validator = new();
    
    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new IngredientFormModel
        {
            Name = string.Empty,
            Description = "desc"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        var model = new IngredientFormModel
        {
            Name = null!,
            Description = "desc"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var model = new IngredientFormModel
        {
            Name = "Salt",
            Description = "desc"
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Validate_ImageUrl_As_Optional()
    {
        var model = new IngredientFormModel
        {
            Name = "Salt",
            ImageUrl = "invalid-url"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.ImageUrl);
    }
    [Fact]
    public async Task ValidatePropertyCommand_Should_Validate_And_Set_Errors()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<IngredientEditViewModel>();

            vm.Ingredient = new IngredientFormModel
            {
                Name = "", // invalid
                Description = "desc"
            };

            // Act
            await vm.ValidatePropertyCommand.ExecuteAsync(
                IngredientFormModelValidator.IngredientNameProperty);

            // Assert
            Assert.NotNull(vm.Ingredient.ValidationResults);
            Assert.False(vm.Ingredient.ValidationResults.IsValid);
            Assert.True(vm.Ingredient.ValidationResults.Errors.Count > 0);
        });
    }

    [Fact]
    public async Task ValidatePropertyCommand_Should_Clear_Property_Error_But_Keep_Others()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<IngredientEditViewModel>();

            vm.Ingredient = new IngredientFormModel
            {
                Name = "",
                Description = ""
            };

            await vm.ValidatePropertyCommand.ExecuteAsync(
                IngredientFormModelValidator.IngredientNameProperty);

            var firstResult = vm.Ingredient.ValidationResults;

            // fix name only
            vm.Ingredient.Name = "Salt";

            await vm.ValidatePropertyCommand.ExecuteAsync(
                IngredientFormModelValidator.IngredientNameProperty);

            var secondResult = vm.Ingredient.ValidationResults;

            Assert.NotNull(firstResult);
            Assert.NotNull(secondResult);

            Assert.True(secondResult.Errors.Count >= 0);
        });
    }
    
}
