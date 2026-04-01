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
            .Custom((value, context) =>
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;     // this is valid state, so stop evaluating further rules
                }

                var result = ImageUrl.CreateObject(value);
                if (result.IsFailure)
                {
                    context.AddFailure(IngredientImageUrlProperty, result.Error.Message);
                }
            });
    }
}
