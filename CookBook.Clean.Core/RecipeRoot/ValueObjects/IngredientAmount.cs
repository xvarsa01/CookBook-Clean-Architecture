namespace CookBook.Clean.Core.RecipeRoot.ValueObjects;

public class IngredientAmount
{
    public decimal Value { get; }
    
    public IngredientAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        
        Value = amount;
    }
    
    public static implicit operator decimal(IngredientAmount amount) => amount.Value;
}
