using Auth.Server.Data.Models;
using Auth.Server.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Auth.Server.Controllers;

[Route("api")]
public class ResourceController(UserManager<ApplicationUser> userManager) : Controller
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("message")]
    public async Task<IActionResult> GetMessage()
    {
        var user = await userManager.FindByIdAsync(User.GetClaim(Claims.Subject))
            .ConfigureAwait(false);
        if (user is null)
        {
            return Challenge(
                authenticationSchemes: OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
                properties: AuthHelper.CreateErrorAuthenticationProperties(Errors.InvalidToken, "The specified access token is bound to an account that no longer exists."));
        }

        return Content($"{user.UserName} has been successfully authenticated.");
    }
}
