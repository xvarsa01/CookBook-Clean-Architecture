namespace CookBook.Clean.Core.RecipeRoot.ValueObjects;

public class RecipeName
{
    public string Value { get; }

    private RecipeName(string value)
    {
        Value = value;
    }

    public static Result<RecipeName> CreateObject(string value)
    {
        return string.IsNullOrWhiteSpace(value) || value.Length < 3
            ? Result.Invalid<RecipeName>("Recipe name must be at least 3 characters.")
            : Result.Ok(new RecipeName(value));
    }

    public static implicit operator string(RecipeName name) => name.Value;
    
    public override string ToString()
    {
        return Value;
    }
}
