using Auth.Server.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Auth.Server.Controllers;

[Route("connect")]
public class UserinfoController(UserManager<ApplicationUser> userManager) : Controller
{
    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("userinfo")]
    [HttpPost("userinfo")]
    [Produces("application/json")]
    public async Task<IActionResult> Userinfo()
    {
        var user = await userManager.FindByIdAsync(User.GetClaim(Claims.Subject))
            .ConfigureAwait(false);
        if (user is null)
        {
            return Challenge(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The specified access token is bound to an account that no longer exists."
                }));
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            [Claims.Subject] = await userManager.GetUserIdAsync(user).ConfigureAwait(false)
        };

        if (User.HasScope(Scopes.Email))
        {
            claims[Claims.Email] = await userManager.GetEmailAsync(user).ConfigureAwait(false);
            claims[Claims.EmailVerified] = await userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false);
        }

        if (User.HasScope(Scopes.Phone))
        {
            claims[Claims.PhoneNumber] = await userManager.GetPhoneNumberAsync(user).ConfigureAwait(false);
            claims[Claims.PhoneNumberVerified] = await userManager.IsPhoneNumberConfirmedAsync(user).ConfigureAwait(false);
        }

        if (User.HasScope(Scopes.Roles))
        {
            claims[Claims.Role] = await userManager.GetRolesAsync(user).ConfigureAwait(false);
        }

        // Note: the complete list of standard claims supported by the OpenID Connect specification
        // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

        return Ok(claims);
    }
}
