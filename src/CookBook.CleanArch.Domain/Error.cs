namespace CookBook.CleanArch.Domain;

public record Error(string Code, string Message, params object[] Arguments)
{
    public static readonly Error NullValue = new("Error.NullValue", "The specific result value is null.");
}
