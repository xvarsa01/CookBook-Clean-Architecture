using CookBook.CleanArch.Domain.Recipe.Errors;

namespace CookBook.CleanArch.Domain.Recipe.ValueObjects;

public class IngredientAmount
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
