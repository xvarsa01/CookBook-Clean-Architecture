using CookBook.Clean.Core.RecipeRoot.Errors;
using CookBook.Clean.Core.Shared.Errors;

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
            ? Result.Invalid<RecipeName>(ValueObjectsErrors.RecipeNameNotInvalidError())
            : Result.Ok(new RecipeName(value));
    }

    public static implicit operator string(RecipeName name) => name.Value;
    
    public override string ToString()
    {
        return Value;
    }
}
