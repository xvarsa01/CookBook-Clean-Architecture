using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.ValueObjects;

public record IngredientAmount : IValueObject<decimal>, IValueObjectFactory<IngredientAmount, decimal>
{
    public decimal Value { get; }
    
    private IngredientAmount(decimal amount)
    {
        Value = amount;
    }
    
    public static Result<IngredientAmount> CreateObject(decimal amount)
    {
        return amount <= 0
            ? Result.Invalid<IngredientAmount>(ValueObjectsErrors.IngredientAmountNotPositiveError())
            : Result.Ok(new IngredientAmount(amount));
    }
    
    public static implicit operator decimal(IngredientAmount amount) => amount.Value;
}
