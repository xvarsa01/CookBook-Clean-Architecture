using CookBook.CleanArch.Domain.Recipe.Errors;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe.ValueObjects;

public class RecipeName : IValueObject<string>, IValueObjectFactory<RecipeName, string>
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
