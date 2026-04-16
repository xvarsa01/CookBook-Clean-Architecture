namespace CookBook.CleanArch.Domain;

public class Result<T> : Result
{
    private readonly T? _value;
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
    
    internal Result(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
    {
        _value = value;
    }

}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    
    private readonly Error? _error;
    public Error Error => IsFailure
        ? _error!
        : throw new InvalidOperationException("The error of a success result can not be accessed.");

    internal Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        _error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error errorMessage) => new(false, errorMessage);
    public static Result<T> Success<T>(T value) => new(true, value, null);
    public static Result<T> Failure<T>(Error errorMessage) => new(false, default, errorMessage);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null 
            ? Success(value)
            : Failure<TValue>(Error.NullValue);
}
