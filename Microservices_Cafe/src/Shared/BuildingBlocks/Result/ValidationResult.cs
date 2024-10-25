using Shared.Abstractions;

namespace Shared.BuildingBlocks.Result;

public sealed class ValidationResult : Result, IValidationResult
{
    public ResultError[] Errors { get; }

    private ValidationResult(ResultError[] errors)
        : base(false, IValidationResult.ValidationError) =>
        Errors = errors;

    public static ValidationResult WithErrors(ResultError[] errors) => new(errors);
}