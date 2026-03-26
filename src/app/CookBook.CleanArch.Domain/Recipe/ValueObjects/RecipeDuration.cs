using CookBook.CleanArch.Domain.Recipe.Errors;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe.ValueObjects;

public class RecipeDuration : IValueObject<TimeSpan>, IValueObjectFactory<RecipeDuration, TimeSpan>
{
    public TimeSpan Value { get; }

    private RecipeDuration(TimeSpan duration)
    {
        Value = duration;
    }

    public static Result<RecipeDuration> CreateObject(TimeSpan duration)
    {
        return duration <= TimeSpan.Zero
            ? Result.Invalid<RecipeDuration>(ValueObjectsErrors.RecipeDurationNotPositiveError())
            : Result.Ok(new RecipeDuration(duration));
    }
    
    public static implicit operator TimeSpan(RecipeDuration duration) => duration.Value;
    
    public int CompareTo(RecipeDuration? other)
        => Value.CompareTo(other!.Value);

    public static bool operator >=(RecipeDuration a, RecipeDuration b)
        => a.Value >= b.Value;

    public static bool operator <=(RecipeDuration a, RecipeDuration b)
        => a.Value <= b.Value;

    public static bool operator >(RecipeDuration a, RecipeDuration b)
        => a.Value > b.Value;

    public static bool operator <(RecipeDuration a, RecipeDuration b)
        => a.Value < b.Value;
}
