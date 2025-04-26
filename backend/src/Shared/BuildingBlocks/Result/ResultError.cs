namespace Shared.BuildingBlocks.Result;

public class ResultError(string code, string message)
{
    public static readonly ResultError None = new(string.Empty, string.Empty);

    public string Code { get; } = code;
    public string Message { get; } = message;
}
