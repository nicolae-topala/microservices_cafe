using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;
using CustomValidationResult = Shared.BuildingBlocks.Result.ValidationResult;

namespace Shared.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommandBase
        where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationFailures = await ValidateAsync(request, cancellationToken);

        if (validationFailures.Length == 0)
        {
            return await next();
        }

        var errors = validationFailures
            .Select(validationFailure =>
                new ResultError(validationFailure.PropertyName, validationFailure.ErrorMessage))
            .ToArray();

        var result = CreateValidationResult<TResponse>(errors);
        return result;
    }

    private async Task<ValidationFailure[]> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(validators
            .Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Distinct()
            .ToArray();

        return validationFailures;
    }

    private static TResult CreateValidationResult<TResult>(ResultError[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (CustomValidationResult.WithErrors(errors) as TResult)!;
        }

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(CustomValidationResult.WithErrors))!
            .Invoke(null, [errors])!;

        return (TResult)validationResult;
    }
}