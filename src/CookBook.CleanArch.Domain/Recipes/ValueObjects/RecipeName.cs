using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.ValueObjects;

public record RecipeName : IValueObject<string>, IValueObjectFactory<RecipeName, string>
{
    public string Value { get; }

    private RecipeName(string value)
    {
        Value = value;
    }

    public static Result<RecipeName> CreateObject(string value)
    {
        return string.IsNullOrWhiteSpace(value) || value.Length < 3
            ? Result.Failure<RecipeName>(ValueObjectsErrors.RecipeNameNotInvalidError())
            : Result.Success(new RecipeName(value));
    }

    public static implicit operator string(RecipeName name) => name.Value;
}
