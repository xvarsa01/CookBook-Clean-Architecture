namespace CookBook.Clean.Core.RecipeRoot.ValueObjects;

public class RecipeDuration
{
    public TimeSpan Value { get; }

    private RecipeDuration(TimeSpan duration)
    {
        Value = duration;
    }

    public static Result<RecipeDuration> CreateObject(TimeSpan duration)
    {
        return duration <= TimeSpan.Zero
            ? Result.Invalid<RecipeDuration>("Duration must be positive")
            : Result.Ok(new RecipeDuration(duration));
    }
    
    public static implicit operator TimeSpan(RecipeDuration duration) => duration.Value;
}
