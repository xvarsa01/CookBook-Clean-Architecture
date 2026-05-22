using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;
using FluentValidation.TestHelper;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Ingredients;

public class IngredientFormValidationTests : MauiTestsBase
{
    private readonly IngredientFormModel.Validator _validator = new();
    
    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = IngredientFormModel.Empty;

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        var model = new IngredientFormModel
        {
            Id = Guid.NewGuid(),
            Name = null!,
            Description = "desc"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var model = IngredientFormModel.Empty;
        model.Name = "Salt";

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Validate_ImageUrl_As_Optional()
    {
        var model = IngredientFormModel.Empty;
        model.ImageUrl = "invalid image url";

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.ImageUrl);
    }
    
    [Fact]
    public void Should_Not_Have_Error_When_ImageUrl_Is_Valid()
    {
        var model = IngredientFormModel.Empty;
        model.ImageUrl = "https://example.com/image.jpg";

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.ImageUrl);
    }
}
