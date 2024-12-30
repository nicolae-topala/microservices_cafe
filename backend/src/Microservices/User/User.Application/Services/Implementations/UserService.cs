using Newtonsoft.Json;
using User.Application.Services.Abstractions;
using User.Shared.DTOs;

namespace User.Application.Services.Implementations;

public class UserService(IHttpClientFactory httpClientFactory) : IUserService
{
    public async Task<UserInfoDto> GetUserInfoAsync()
    {
        var httpClient = httpClientFactory.CreateClient("UserMicroservice");
        var response = await httpClient.GetAsync("/connect/userinfo");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UserInfoDto>(content);
    }
}
