using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using FluentValidation;

namespace CookBook.CleanArch.Presentation.MauiApplication.Validations;

public class IngredientDetailModelValidator : AbstractValidator<IngredientFormModel>
{
    public static string IngredientNameProperty => nameof(IngredientFormModel.Name);
    public static string IngredientImageUrlProperty => nameof(IngredientFormModel.ImageUrl);

    public IngredientDetailModelValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("The ingredient name must not be empty")
            .NotEmpty().WithMessage("The ingredient name must not be empty");

        RuleFor(x => x.ImageUrl)
            .IsValidOptionalValueObject<IngredientFormModel, ImageUrl>();
    }
}
