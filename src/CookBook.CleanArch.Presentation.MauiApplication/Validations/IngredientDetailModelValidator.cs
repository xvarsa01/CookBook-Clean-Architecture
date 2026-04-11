using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using FluentValidation;

namespace CookBook.CleanArch.Presentation.MauiApplication.Validations;

public class IngredientDetailModelValidator : AbstractValidator<IngredientDetailModel>
{
    public static string IngredientNameProperty => nameof(IngredientDetailModel.Name);
    public static string IngredientImageUrlProperty => nameof(IngredientDetailModel.ImageUrl);

    public IngredientDetailModelValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("The ingredient name must not be empty")
            .NotEmpty().WithMessage("The ingredient name must not be empty");

        RuleFor(x => x.ImageUrl)
            .IsValidOptionalValueObject<IngredientDetailModel, ImageUrl>();
    }
}
