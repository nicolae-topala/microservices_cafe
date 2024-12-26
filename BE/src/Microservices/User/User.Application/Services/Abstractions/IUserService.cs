using User.Shared.DTOs;

namespace User.Application.Services.Abstractions;

public interface IUserService
{
    Task<UserInfoDto> GetUserInfoAsync();
}
