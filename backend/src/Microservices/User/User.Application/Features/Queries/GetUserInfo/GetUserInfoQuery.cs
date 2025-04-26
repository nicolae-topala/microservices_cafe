using Shared.Abstractions.Messaging;
using Shared.Abstractions.Messaging.ResultType;
using User.Shared.DTOs;

namespace User.Application.Features.Queries.GetUserInfo;

public record GetUserInfoQuery : IResultQuery<UserInfoDto>;