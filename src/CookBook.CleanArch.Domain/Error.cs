using System.Diagnostics.CodeAnalysis;

namespace CookBook.CleanArch.Domain;

public sealed record Error
{
    public required string Code { get; init; }
    public required string Message { get; init; }
    public object[] Arguments { get; init; } = [];

    [SetsRequiredMembers]
    public Error(string code, string message, params object[] arguments)
    {
        Code = code;
        Message = message;
        Arguments = arguments;
    }

    public static readonly Error NullValue = new("Error.NullValue", "The specific result value is null.");
    
    public bool Equals(Error? other)
        => other is not null
           && Code == other.Code
           && Message == other.Message;

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }
}
