using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using FluentValidation;

namespace CookBook.CleanArch.Presentation.MauiApplication.Validations;

public class RecipeIngredientListModelValidator : AbstractValidator<RecipeIngredientListModel>
{
    public static string IngredientIdProperty => nameof(RecipeIngredientListModel.IngredientId);
    public static string IngredientAmountProperty => nameof(RecipeIngredientListModel.Amount);
    public static string IngredientUnitProperty => nameof(RecipeIngredientListModel.Unit);

    public RecipeIngredientListModelValidator()
    {
        RuleFor(x => x.IngredientId)
            .NotEqual(Guid.Empty)
            .WithMessage("The ingredient must be selected");
        
        RuleFor(x => x.Amount)
            .Custom((value, context) =>
            {
                var result = IngredientAmount.CreateObject(value);
                if (result.IsFailure)
                {
                    context.AddFailure(IngredientAmountProperty, result.Error.Message);
                }
            });
        
        RuleFor(x => x.Amount)
            .Custom((value, context) =>
            {
                var result = IngredientAmount.CreateObject(value);
                if (result.IsFailure)
                {
                    context.AddFailure(IngredientAmountProperty, result.Error.Message);
                }
            });
        
        RuleFor(x => x.Unit)
            .NotEqual(MeasurementUnit.Unit)
            .WithMessage("The recipe type must be selected");
    }
}
