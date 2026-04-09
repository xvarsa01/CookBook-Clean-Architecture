using CookBook.CleanArch.Domain.Shared;
using FluentValidation;

namespace CookBook.CleanArch.Presentation.MauiApplication.Validations;

public static class ValidationResultExtensions
{
    public static IRuleBuilderOptionsConditions<T, string> IsValidValueObject<T, TValueObject>(this IRuleBuilderInitial<T, string> ruleBuilder) where TValueObject : IValueObject<string>, IValueObjectFactory<TValueObject, string>
    {
        return ruleBuilder.IsValidValueObject<T, string, TValueObject>();
    }
  
    public static IRuleBuilderOptionsConditions<T, TimeSpan> IsValidValueObject<T, TValueObject>(this IRuleBuilderInitial<T, TimeSpan> ruleBuilder) where TValueObject : IValueObject<TimeSpan>, IValueObjectFactory<TValueObject, TimeSpan>
    {
        return ruleBuilder.IsValidValueObject<T, TimeSpan, TValueObject>();
    }
    
    public static IRuleBuilderOptionsConditions<T, decimal> IsValidValueObject<T, TValueObject>(this IRuleBuilderInitial<T, decimal> ruleBuilder) where TValueObject : IValueObject<decimal>, IValueObjectFactory<TValueObject, decimal>
    {
        return ruleBuilder.IsValidValueObject<T, decimal, TValueObject>();
    }
  
  
    public static IRuleBuilderOptionsConditions<T, TProperty> IsValidValueObject<T, TProperty, TValueObject>(this IRuleBuilderInitial<T, TProperty> ruleBuilder) where TValueObject : IValueObject<TProperty>, IValueObjectFactory<TValueObject, TProperty>
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = TValueObject.CreateObject(value);
            if (result.IsFailure)
            {
                context.AddFailure(result.Error.Message);
            }
        });
    }
    
    public static IRuleBuilderOptionsConditions<T, string?> IsValidOptionalValueObject<T, TValueObject>(
        this IRuleBuilderInitial<T, string?> ruleBuilder)
        where TValueObject : IValueObject<string>, IValueObjectFactory<TValueObject, string>
    {
        return ruleBuilder.Custom((value, context) =>
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var result = TValueObject.CreateObject(value);
            if (result.IsFailure)
            {
                context.AddFailure(result.Error.Message);
            }
        });
    }
  
}
