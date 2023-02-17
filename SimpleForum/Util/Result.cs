namespace SimpleForum.Util;

public enum ErrorType
{
    BadRequest,
    NotFound,
    Unauthorized,
    Forbidden
}

public record Error(string Detail, ErrorType Type);

/// <summary>
/// Displays the 'result' of an operation, allowing it's success to be checked and the reason for failure if applicable.
/// </summary>
public class Result
{
    /// <summary>
    /// Creates a new result object
    /// </summary>
    /// <param name="success">Whether the operation was successful or not</param>
    /// <param name="error">The error, null if successful</param>
    protected Result(bool success, Error? error)
    {
        Success = success;
        Error = error!;
    }

    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool Success { get; }
    
    /// <summary>
    /// The error if unsuccessful
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Creates a success result object
    /// </summary>
    /// <returns></returns>
    public static Result Successful() => new(true, null);

    /// <summary>
    /// Creates a failure result object
    /// </summary>
    /// <param name="error">The reason for failure</param>
    /// <param name="type">The type of failure</param>
    /// <returns></returns>
    public static Result Failure(string error, ErrorType type) => new(false, new Error(error, type));

    /// <summary>
    /// Creates a success result object for an operation with a result value
    /// </summary>
    /// <param name="value">The value of the result</param>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <returns></returns>
    public static Result<T> Successful<T>(T value) => new(true, null, value);

    /// <summary>
    /// Creates a failure object for an operation with a value
    /// </summary>
    /// <param name="error">The reason for failure</param>
    /// <param name="type">The type of failure</param>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <returns></returns>
    public static Result<T> Failure<T>(string error, ErrorType type) => new(false, new Error(error, type), default);
}

/// <summary>
/// Displays the 'result' of operation and contains the value of the result if successful.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Result<T> : Result
{
    protected internal Result(bool success, Error? error, T? value) : base(success, error)
    {
        Value = value;
    }

    /// <summary>
    /// The value of the result. Null if the operation was unsuccessful.
    /// </summary>
    public T? Value { get; }
}