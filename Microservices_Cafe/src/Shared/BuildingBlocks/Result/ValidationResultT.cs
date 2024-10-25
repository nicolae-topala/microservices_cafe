using Shared.Abstractions;

namespace Shared.BuildingBlocks.Result;
public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    public ResultError[] Errors { get; }

    private ValidationResult(ResultError[] errors)
        : base(default, false, IValidationResult.ValidationError) =>
        Errors = errors;

    public static ValidationResult<TValue> WithErrors(ResultError[] errors) => new(errors);
}