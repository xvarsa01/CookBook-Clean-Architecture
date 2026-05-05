namespace CookBook.CleanArch.Domain;

public class Result<T> : Result
{
    public T Value => IsSuccess
        ? field!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
    
    internal Result(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
    {
        Value = value;
    }

}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error => IsFailure
        ? field!
        : throw new InvalidOperationException("The error of a success result can not be accessed.");

    internal Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error errorMessage) => new(false, errorMessage);
    public static Result<T> Success<T>(T value) => new(true, value, null);
    public static Result<T> Failure<T>(Error errorMessage) => new(false, default, errorMessage);

    public static Result<TValue> Create<TValue>(TValue value) => Success(value);
}
