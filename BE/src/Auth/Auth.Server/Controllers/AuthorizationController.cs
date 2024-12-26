using Auth.Server.Data.Models;
using Auth.Server.Helpers;
using Auth.Server.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Immutable;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Auth.Server.Controllers;

[Route("connect")]
public class AuthorizationController(
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictAuthorizationManager authorizationManager,
    IOpenIddictScopeManager scopeManager,
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet("authorize")]
    [HttpPost("authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var result = await HttpContext.AuthenticateAsync()
            .ConfigureAwait(false);

        var isAuthenticated = AuthHelper.IsAuthenticated(result, request);
        if (!isAuthenticated)
        {
            return HandleLoginPrompt(request);
        }

        if (result.Principal is null)
        {
            throw new InvalidOperationException("The user principal is null.");
        }

        var user = await userManager.GetUserAsync(result.Principal).ConfigureAwait(false) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        if (request.ClientId is null)
        {
            throw new InvalidOperationException("Details concerning the calling client application cannot be found.");
        }

        var application = await applicationManager.FindByClientIdAsync(request.ClientId, cancellationToken).ConfigureAwait(false) ??
            throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

        var authorizations = await GetAuthorizationsAsync(user, application, request, cancellationToken)
            .ConfigureAwait(false);

        var consentType = await applicationManager.GetConsentTypeAsync(application, cancellationToken)
            .ConfigureAwait(false);

        // Allow only explicit consent
        if (consentType != ConsentTypes.Explicit || request.HasPrompt(Prompts.None))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: AuthHelper.CreateErrorAuthenticationProperties(Errors.InvalidClient, "Only clients with explicit consent type are allowed.")
                );
        }

        if (authorizations.Count is not 0 && !request.HasPrompt(Prompts.Consent))
        {
            return await HandleAuthorizationAsync(user, application, authorizations, request, cancellationToken);
        }

        // In every other case, render the consent form.
        var applicationName = await applicationManager.GetLocalizedDisplayNameAsync(application, cancellationToken)
                .ConfigureAwait(false);
        return View(new AuthorizeViewModel
        {
            ApplicationName = applicationName ?? "Unknown",
            Scope = request.Scope ?? "Unknown"
        });
    }

    [Authorize]
    [HttpPost("authorize")]
    [FormValueRequired("submit.Accept")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Accept(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var user = await userManager.GetUserAsync(User).ConfigureAwait(false) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        var application = await applicationManager.FindByClientIdAsync(request.ClientId, cancellationToken).ConfigureAwait(false) ??
            throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

        var authorizations = await GetAuthorizationsAsync(user, application, request, cancellationToken)
            .ConfigureAwait(false);

        var isConsentTypeExternal = await applicationManager.HasConsentTypeAsync(application, ConsentTypes.External, cancellationToken)
            .ConfigureAwait(false);
        if (authorizations.Count is 0 && isConsentTypeExternal)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: AuthHelper.CreateErrorAuthenticationProperties(Errors.ConsentRequired, "The logged in user is not allowed to access this client application."));
        }

        return await HandleAuthorizationAsync(user, application, authorizations, request, cancellationToken);
    }

    [Authorize]
    [HttpPost("authorize")]
    [FormValueRequired("submit.Deny")]
    [ValidateAntiForgeryToken]
    public IActionResult Deny() => Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

    [HttpGet("logout")]
    public IActionResult Logout() => View();

    [HttpPost("logout")]
    [ActionName(nameof(Logout))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost()
    {
        await signInManager.SignOutAsync().ConfigureAwait(false);

        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            });
    }

    [HttpPost("token")]
    [Produces("application/json")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Exchange(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");


        if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
        {
            throw new InvalidOperationException("The specified grant type is not supported.");
        }

        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
            .ConfigureAwait(false);

        var user = await userManager.FindByIdAsync(result.Principal.GetClaim(Claims.Subject))
            .ConfigureAwait(false);

        if (user is null)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: AuthHelper.CreateErrorAuthenticationProperties(Errors.InvalidGrant, "The token is no longer valid."));
        }

        if (!await signInManager.CanSignInAsync(user).ConfigureAwait(false))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: AuthHelper.CreateErrorAuthenticationProperties(Errors.InvalidGrant, "The user is no longer allowed to sign in."));
        }

        var identity = new ClaimsIdentity(result.Principal.Claims,
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);

        await SetIdentityClaimsAsync(user, identity, request, cancellationToken).ConfigureAwait(false);

        identity.SetDestinations(AuthHelper.GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private IActionResult HandleLoginPrompt(OpenIddictRequest request)
    {
        if (request.HasPrompt(Prompts.None))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: AuthHelper.CreateErrorAuthenticationProperties(Errors.LoginRequired, "The user is not logged in."));
        }

        // To avoid endless login -> authorization redirects, the prompt=login flag                                                                         
        // is removed from the authorization request payload before redirecting the user.
        var prompt = string.Join(" ", request.GetPrompts().Remove(Prompts.Login));

        var parameters = AuthHelper.ParseOAuthParameters(HttpContext, [Parameters.Prompt]);
        parameters.Add(KeyValuePair.Create(Parameters.Prompt, new StringValues(prompt)));

        return Challenge(new AuthenticationProperties
        {
            RedirectUri = AuthHelper.BuildRedirectUrl(HttpContext.Request, parameters)
        });
    }

    private async Task<List<object>> GetAuthorizationsAsync(ApplicationUser user, object application, OpenIddictRequest request, CancellationToken cancellationToken) => 
        await authorizationManager.FindAsync(
            subject: await userManager.GetUserIdAsync(user).ConfigureAwait(false),
            client: await applicationManager.GetIdAsync(application, cancellationToken).ConfigureAwait(false),
            status: Statuses.Valid,
            type: AuthorizationTypes.Permanent,
            scopes: request.GetScopes(),
            cancellationToken: cancellationToken).ToListAsync().ConfigureAwait(false);


    private async Task<object> GetAuthorizationAsync(ClaimsIdentity identity, ApplicationUser user, object application, List<object> authorizations, 
        CancellationToken cancellationToken)
    {
        var authorization = authorizations.LastOrDefault();
        authorization ??= await authorizationManager.CreateAsync(
            identity: identity,
            subject: await userManager.GetUserIdAsync(user).ConfigureAwait(false),
            client: await applicationManager.GetIdAsync(application, cancellationToken).ConfigureAwait(false),
            type: AuthorizationTypes.Permanent,
            scopes: identity.GetScopes(),
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return authorization;
    }

    private async Task<IActionResult> HandleAuthorizationAsync(ApplicationUser user, object application, List<object> authorizations, OpenIddictRequest request, 
        CancellationToken cancellationToken)
    {
        var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

        await SetIdentityClaimsAsync(user, identity, request, cancellationToken).ConfigureAwait(false);

        var authorization = await GetAuthorizationAsync(identity, user, application, authorizations, cancellationToken)
            .ConfigureAwait(false);
        var authorizationId = await authorizationManager.GetIdAsync(authorization, cancellationToken)
            .ConfigureAwait(false);

        identity.SetAuthorizationId(authorizationId);
        identity.SetDestinations(AuthHelper.GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task SetIdentityClaimsAsync(ApplicationUser user, ClaimsIdentity identity, OpenIddictRequest request, CancellationToken cancellationToken)
    {
        var userId = await userManager.GetUserIdAsync(user).ConfigureAwait(false);
        var email = await userManager.GetEmailAsync(user).ConfigureAwait(false);
        var name = await userManager.GetUserNameAsync(user).ConfigureAwait(false);
        var username = await userManager.GetUserNameAsync(user).ConfigureAwait(false);
        var roles = (await userManager.GetRolesAsync(user).ConfigureAwait(false)).ToImmutableArray();

        identity.SetClaim(Claims.Subject, userId)
                .SetClaim(Claims.Email, email)
                .SetClaim(Claims.Name, name)
                .SetClaim(Claims.PreferredUsername, username)
                .SetClaims(Claims.Role, roles);

        // The granted scopes match the requested scope
        // but if wanted to allow the user to uncheck specific scopes
        // restrict the list of scopes before calling SetScopes.
        var scopes = request.GetScopes();
        identity.SetScopes(scopes);

        var resources = await scopeManager.ListResourcesAsync(identity.GetScopes(), cancellationToken)
            .ToListAsync()
            .ConfigureAwait(false);
        identity.SetResources(resources);
    }
}
