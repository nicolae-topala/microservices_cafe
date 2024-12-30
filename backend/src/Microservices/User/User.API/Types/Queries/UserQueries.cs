using HotChocolate.Authorization;
using MediatR;
using User.Application.Features.Queries.GetUserInfo;
using User.Shared.DTOs;

namespace User.API.Types.Queries;

[QueryType]
public class UserQueries
{
    [Authorize]
    public Task<UserInfoDto> GetUserInfo(ISender sender)
    {
        var result = sender.Send(new GetUserInfoQuery())
            .ContinueWith(t => t.Result.Value);
        return result;
    }
}