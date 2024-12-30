using Auth.Server.Data.Models;
using Auth.Server.DTOs;
using Auth.Server.Services.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Auth.Server.Controllers;

[Route("connect")]
public class UserinfoController(
    UserManager<ApplicationUser> userManager,
    IBlobStorageService blobStorageService)
    : Controller
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

        if (User.HasScope(Scopes.Profile))
        {
            claims[Claims.Username] = await userManager.GetUserNameAsync(user).ConfigureAwait(false);
            claims["avatarUrl"] = user.AvatarUrl;
        }

        // Note: the complete list of standard claims supported by the OpenID Connect specification
        // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

        return Ok(claims);
    }

    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [HttpPatch("userinfo/update-profile")]
    [Produces("application/json")]
    public async Task<IActionResult> UpdateUserinfo([FromBody] UpdateUserinfoDto request)
    {
        var user = await userManager.FindByIdAsync(User.GetClaim(Claims.Subject)).ConfigureAwait(false);
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

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailUpdateResult = await userManager.SetEmailAsync(user, request.Email).ConfigureAwait(false);
            if (!emailUpdateResult.Succeeded)
            {
                return BadRequest(new { error = "update_failed", errors = emailUpdateResult.Errors });
            }
        }

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var phoneUpdateResult = await userManager.SetPhoneNumberAsync(user, request.PhoneNumber).ConfigureAwait(false);
            if (!phoneUpdateResult.Succeeded)
            {
                return BadRequest(new { error = "update_failed", errors = phoneUpdateResult.Errors });
            }
        }

        return Ok(new { message = "User information updated successfully." });
    }

    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [HttpPost("userinfo/profile-picture")]
    [Produces("application/json")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
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

        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            await blobStorageService.DeleteAsync(user.Id.ToString());
        }

        var filePath = await blobStorageService.UploadAsync(file, user.Id.ToString());
        user.AvatarUrl = filePath; 
        await userManager.UpdateAsync(user).ConfigureAwait(false);

        return Ok(new { message = "Profile picture updated successfully.", url = filePath });
    }

    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [HttpDelete("userinfo/profile-picture")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteProfilePicture()
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

        if (string.IsNullOrEmpty(user.AvatarUrl))
        {
            return BadRequest("No profile picture assigned to the account.");
        }

        await blobStorageService.DeleteAsync(user.Id.ToString());
        user.AvatarUrl = null;
        await userManager.UpdateAsync(user).ConfigureAwait(false);

        return Ok("Profile picture deleted successfully.");
    }
}
