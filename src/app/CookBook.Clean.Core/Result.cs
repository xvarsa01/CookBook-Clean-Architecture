namespace CookBook.Clean.Core;

public class Result<T> : Result
{
    private readonly T? _value;
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
    
    internal Result(bool isSuccess, T? value, string? error) : base(isSuccess, error)
    {
        _value = value;
    }

}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    internal Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Ok() => new(true, null);
    public static Result NotFound(string message) => new(false, message);
    public static Result Invalid(string message) => new(false, message);
    public static Result<T> Ok<T>(T value) => new(true, value, null);
    public static Result<T> NotFound<T>(string message) => new(false, default, message);
    public static Result<T> Invalid<T>(string message) => new(false, default, message);
}
