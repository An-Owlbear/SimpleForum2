namespace SimpleForum.Util;

public enum ErrorType
{
    BadRequest,
    NotFound,
    Unauthorized,
    Forbidden
}

public record Error(string Detail, ErrorType Type);

public class Result
{
    protected Result(bool success, Error? error)
    {
        Success = success;
        Error = error;
    }

    public bool Success { get; }
    public Error Error { get; }

    public static Result Successful() => new(true, null);

    public static Result Failure(string error, ErrorType type) => new(false, new Error(error, type));

    public static Result<T> Successful<T>(T value) => new(true, null, value);

    public static Result<T> Failure<T>(string error, ErrorType type) => new(false, new Error(error, type), default);
}

public class Result<T> : Result
{
    protected internal Result(bool success, Error? error, T? value) : base(success, error)
    {
        Value = value;
    }

    public T? Value { get; }
}