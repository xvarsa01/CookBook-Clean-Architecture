namespace CookBook.Clean.Core.Shared.Exceptions;

public sealed class InvalidImageUrlException : DomainException
{
    public InvalidImageUrlException(string invalidUrl) : base($"Invalid image URL format : {invalidUrl}")
    {
    }
}
