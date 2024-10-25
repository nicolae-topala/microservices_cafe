using Shared.BuildingBlocks.Result;

namespace Shared.Errors;

public static class CommonErrors
{
    public static readonly ResultError NullValue = new("NullValue", "The specified result value is null.");
}
