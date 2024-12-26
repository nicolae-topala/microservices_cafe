using Shared.BuildingBlocks.Result;

namespace Shared.Abstractions;

public interface IValidationResult
{
    public static readonly ResultError ValidationError = new(
        "Validation",
        "A validation problem occured.");

    ResultError[] Errors { get; }
}
