using Shared.Abstractions.Messaging;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;
using User.Application.Services.Abstractions;
using User.Shared.DTOs;

namespace User.Application.Features.Queries.GetUserInfo;

public class GetUserInfoQueryHandler(IUserService userService)
    : IResultQueryHandler<GetUserInfoQuery, UserInfoDto>
{
    public async Task<Result<UserInfoDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var result = await userService.GetUserInfoAsync();
        
        if (result is null)
        {
            Result.Failure(new ResultError("",""));
        }

        return Result.Success(result);
    }
}