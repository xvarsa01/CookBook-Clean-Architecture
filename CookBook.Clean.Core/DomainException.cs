namespace CookBook.Clean.Core;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
        
    }
}
