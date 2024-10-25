using FluentValidation;
using MediatR;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Abstractions.Behaviors;

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
        if (!validators.Any())
        {
            return await next();
        }

        // Validate
        var context = new ValidationContext<TRequest>(request);
        var errors = validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => 
                new ResultError(validationFailure.PropertyName, validationFailure.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Length > 0) 
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(ResultError[] errors)
        where TResult : Result
    {
        if(typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, [errors])!;

        return (TResult)validationResult;
    }
}