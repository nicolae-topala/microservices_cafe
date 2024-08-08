using MicroservicesCafe.Shared.Errors;

namespace MicroservicesCafe.Shared.BuildingBlocks.Result;
public class Result
{
    public Error Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Success<T>(T value) => new(value, true, Error.None);
    public static Result<T> Failure<T>(Error error) => new(default!, false, error);
    public static Result<T> Create<T>(T? value) => value is not null ? Success(value) : Failure<T>(CommonErrors.NullValue);

    protected Result(bool isSuccess, Error error)
    {
        // Successful must not have an error
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        // Failed mush have an error
        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

}
