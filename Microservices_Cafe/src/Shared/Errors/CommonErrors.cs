using Shared.BuildingBlocks.Result;

namespace Shared.Errors;

public static class CommonErrors
{
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public static readonly Error InvitingCreator = new(
        "Gathering.InvitingCreator",
        "Can't send invitation to the gathering creator.");
}
