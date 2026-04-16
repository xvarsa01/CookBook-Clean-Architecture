using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes.ValueObjects;

public record RecipeDuration : IValueObject<TimeSpan>, IValueObjectFactory<RecipeDuration, TimeSpan>
{
    public TimeSpan Value { get; }

    private RecipeDuration(TimeSpan duration)
    {
        Value = duration;
    }

    public static Result<RecipeDuration> CreateObject(TimeSpan duration)
    {
        return duration <= TimeSpan.Zero
            ? Result.Failure<RecipeDuration>(ValueObjectsErrors.RecipeDurationNotPositiveError())
            : Result.Success(new RecipeDuration(duration));
    }
    
    public static implicit operator TimeSpan(RecipeDuration duration) => duration.Value;
    
    public static bool operator >=(RecipeDuration a, RecipeDuration b)
        => a.Value >= b.Value;

    public static bool operator <=(RecipeDuration a, RecipeDuration b)
        => a.Value <= b.Value;

    public static bool operator >(RecipeDuration a, RecipeDuration b)
        => a.Value > b.Value;

    public static bool operator <(RecipeDuration a, RecipeDuration b)
        => a.Value < b.Value;
}
