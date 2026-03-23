namespace CookBook.Clean.Application;

public class UseCaseResult<T>
{
    public bool Success { get; }
    public string? Error { get; }
    public T? Value { get; }

    private UseCaseResult(bool success, T? value, string? error)
    {
        Success = success;
        Value = value;
        Error = error;
    }

    public static UseCaseResult<T> Ok(T value) => new(true, value, null);
    public static UseCaseResult<T> NotFound(string message) => new(false, default, message);
    public static UseCaseResult<T> Invalid(string message) => new(false, default, message);
}

public class UseCaseResult
{
    public bool Success { get; }
    public string? Error { get; }

    private UseCaseResult(bool success, string? error)
    {
        Success = success;
        Error = error;
    }

    public static UseCaseResult Ok() => new(true, null);
    public static UseCaseResult NotFound(string message) => new(false, message);
    public static UseCaseResult Invalid(string message) => new(false, message);
}
