namespace CookBook.CleanArch.Domain;

public static class ResultExtensions
{
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error errorMessage)
    {
        if (result.IsFailure)
            return result;

        return predicate(result.Value)
            ? result
            : Result.Invalid<T>(errorMessage);
    }

    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Func<T, Error> errorFactory)
    {
        if (result.IsFailure)
            return result;

        return predicate(result.Value)
            ? result
            : Result.Invalid<T>(errorFactory(result.Value));
    }

    public static async Task<Result<T>> Ensure<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate, Func<T, Error> errorFactory)
    {
        var result = await resultTask;
        return result.Ensure(predicate, errorFactory);
    }
    
    public static async Task<Result<TIn>> EnsureNotNull<TIn>(this Task<TIn?> objTask, Error? error = null)
    {
        var obj = await objTask;
        return obj is null ? Result.Invalid<TIn>(error ?? Error.NullValue) : Result.Ok(obj);
    }

    public static async Task<Result<TIn>> EnsureNotNullNotFound<TIn>(this Task<TIn?> objTask, Error error)
    {
        var obj = await objTask;
        return obj is null ? Result.NotFound<TIn>(error) : Result.Ok(obj);
    }

    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }
    
    public static async Task<Result<TIn>> Tap<TIn>(this Result<TIn> result, Func<Task> func)
    {
        if (result.IsSuccess)
        {
            await func();
        }

        return result;
    }
    
    
    public static async Task<Result<TIn>> Tap<TIn>(this Task<Result<TIn>> resultTask, Func<TIn, Result> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            var actionResult = action(result.Value);
            if (actionResult.IsFailure)
            {
                return Result.Invalid<TIn>(actionResult.Error);
            }
        }

        return result;
    }
    
    public static async Task<Result<TIn>> Tap<TIn>(this Task<Result<TIn>> resultTask, Action<TIn> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    public static async Task<Result<TIn>> Tap<TIn>(this Task<Result<TIn>> resultTask, Func<Task> func)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            await func();
        }

        return result;
    }
    
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mappingFunc)
    {
        return result.IsSuccess
            ? Result.Ok(mappingFunc(result.Value))
            : Result.Invalid<TOut>(result.Error);
    }
    
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        return result.IsSuccess
            ? func(result.Value)
            : Result.Invalid<TOut>(result.Error);
    }

    public static Result ToResult<TIn>(this Result<TIn> result)
    {
        return result.IsSuccess
            ? Result.Ok()
            : Result.Invalid(result.Error);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<Result<TOut>>> func)
    {
        var result = await resultTask;
        if (result.IsFailure)
            return Result.Invalid<TOut>(result.Error);

        return await func(result.Value);
    }
}
