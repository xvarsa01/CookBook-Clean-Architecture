namespace CookBook.CleanArch.Domain;

public record Error(string Message)
{
    public static readonly Error NullValue = new("The specific result value is null.");
}
