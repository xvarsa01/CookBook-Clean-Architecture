using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using FluentValidation;

namespace CookBook.CleanArch.Presentation.MauiApplication.Validations;

public class RecipeDetailModelValidator : AbstractValidator<RecipeDetailModel>
{
    public static string RecipeNameProperty => nameof(RecipeDetailModel.Name);
    public static string RecipeDurationProperty => nameof(RecipeDetailModel.Duration);
    public static string RecipeImageUrlProperty => nameof(RecipeDetailModel.ImageUrl);
    public static string RecipeRecipeTypeProperty => nameof(RecipeDetailModel.RecipeType);

    public RecipeDetailModelValidator()
    {
        RuleFor(x => x.Name)
            .IsValidValueObject<RecipeDetailModel, RecipeName>();

        RuleFor(x => x.Duration)
            .IsValidValueObject<RecipeDetailModel, RecipeDuration>();

        RuleFor(x => x.RecipeType)
            .NotEqual(RecipeType.None)
            .WithMessage("The recipe type must be selected");

        RuleFor(x => x.ImageUrl)
            .IsValidOptionalValueObject<RecipeDetailModel, ImageUrl>();
    }
}
