namespace MicroservicesCafe.Shared.BuildingBlocks.Result;

public class Result<T> : Result
{
    private readonly T? _value;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    protected internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error) =>
        _value = value;

    public static implicit operator Result<T>(T? value) => Create(value);
}
