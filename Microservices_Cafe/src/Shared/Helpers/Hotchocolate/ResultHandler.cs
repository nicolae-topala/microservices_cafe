using Shared.Abstractions;
using Shared.BuildingBlocks.Result;
using HotChocolate;

namespace Shared.Helpers.Hotchocolate;

public static class ResultHandler
{
    public static FieldResult<bool> HandleResponse(Result result)
    {
        if (result.IsFailure)
        {
            return new FieldResult<bool>(result.Error);
        }
        return result.IsSuccess;
    }

    public static FieldResult<TResponse> HandleResponse<TResponse>(Result<TResponse> result)
    {
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return result.Value;
    }

    private static FieldResult<TResponse> HandleFailure<TResponse>(Result<TResponse> result) =>
        result switch
        {
            { IsSuccess: true } => 
                throw new InvalidOperationException("Can't handle a success of a failure."),
            IValidationResult validationResult => new FieldResult<TResponse>(validationResult.Errors),
            _ => new FieldResult<TResponse>(result.Error)
        };
}