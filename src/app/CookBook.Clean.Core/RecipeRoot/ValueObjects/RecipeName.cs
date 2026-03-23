namespace CookBook.Clean.Core.RecipeRoot.ValueObjects;

public class RecipeName
{
    public string Value { get; }

    public RecipeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
            throw new ArgumentException("Recipe name must be at least 3 characters.");

        Value = value;
    }

    public static implicit operator string(RecipeName name) => name.Value;
    
    public override string ToString()
    {
        return Value;
    }
}
