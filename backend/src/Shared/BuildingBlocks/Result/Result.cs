using Shared.Errors;

namespace Shared.BuildingBlocks.Result;
public class Result
{
    public ResultError Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, ResultError.None);
    public static Result<T> Success<T>(T value) => new(value, true, ResultError.None);

    public static Result Failure(ResultError error) => new(false, error);
    public static Result<T> Failure<T>(ResultError error) => new(default!, false, error);

    public static Result<T> Create<T>(T? value) => value is not null ? Success(value) : Failure<T>(CommonErrors.NullValue);

    protected Result(bool isSuccess, ResultError error)
    {
        // Successful must not have an error
        if (isSuccess && error != ResultError.None)
        {
            throw new InvalidOperationException();
        }

        // Failed must have an error
        if (!isSuccess && error == ResultError.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

}
