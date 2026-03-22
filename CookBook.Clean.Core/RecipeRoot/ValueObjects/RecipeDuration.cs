namespace CookBook.Clean.Core.RecipeRoot.ValueObjects;

public class RecipeDuration
{
    public TimeSpan Value { get; }
    
    public RecipeDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive");
        
        Value = duration;
    }
    
    public static implicit operator TimeSpan(RecipeDuration duration) => duration.Value;
    
}
