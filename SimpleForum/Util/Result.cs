namespace SimpleForum.Util;

public class Result
{
    protected Result(bool success, string? error)
    {
        Success = success;
        Error = error;
    }

    public bool Success { get; }
    public string? Error { get; }

    public static Result Successful() => new Result(true, null);

    public static Result Failure(string error) => new Result(false, error);

    public static Result<T> Successful<T>(T value) => new Result<T>(true, null, value);

    public static Result<T> Failure<T>(string error) => new Result<T>(false, error, default);
}

public class Result<T> : Result
{
    protected internal Result(bool success, string? error, T? value) : base(success, error)
    {
        Value = value;
    }

    public T? Value { get; }
}