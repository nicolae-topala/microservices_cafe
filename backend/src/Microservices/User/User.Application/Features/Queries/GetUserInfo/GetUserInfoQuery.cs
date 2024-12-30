using Shared.Abstractions.Messaging;
using User.Shared.DTOs;

namespace User.Application.Features.Queries.GetUserInfo;

public record GetUserInfoQuery : IQuery<UserInfoDto>;