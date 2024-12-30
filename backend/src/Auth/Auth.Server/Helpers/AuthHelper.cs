using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using OpenIddict.Server.AspNetCore;

namespace Auth.Server.Helpers
{
    public static class AuthHelper
    {
        public static IDictionary<string, StringValues> ParseOAuthParameters(HttpContext httpContext, List<string>? excluding = null)
        {
            excluding ??= [];

            var parameters = httpContext.Request.HasFormContentType
                ? httpContext.Request.Form
                    .Where(x => !excluding.Contains(x.Key))
                    .ToDictionary(x => x.Key, x => x.Value)
                : httpContext.Request.Query
                    .Where(x => !excluding.Contains(x.Key))
                    .ToDictionary(x => x.Key, x => x.Value);

            return parameters;
        }

        public static string BuildRedirectUrl(HttpRequest request, IDictionary<string, StringValues> oAuthParameters)
        {
            var url = request.PathBase + request.Path + QueryString.Create(oAuthParameters);
            return url;
        }

        // Try to retrieve the user principal stored in the authentication cookie and redirect
        // the user agent to the login page (or to an external provider) in the following cases:
        //  - If the user principal can't be extracted or the cookie is too old.
        //  - If prompt=login was specified by the client application.
        //  - If a max_age parameter was provided and the authentication cookie is not considered "fresh" enough.
        public static bool IsAuthenticated(AuthenticateResult authenticateResult, OpenIddictRequest request)
        {
            if (authenticateResult is null || !authenticateResult.Succeeded)
            {
                return false;
            }

            if (request.HasPrompt(Prompts.Login))
            {
                return false;
            }

            if (request.MaxAge is not null && authenticateResult.Properties is not null)
            {
                var maxAgeSeconds = TimeSpan.FromSeconds(request.MaxAge.Value);

                var expired = !authenticateResult.Properties.IssuedUtc.HasValue ||
                              DateTimeOffset.UtcNow - authenticateResult.Properties.IssuedUtc > maxAgeSeconds;
                if (expired)
                {
                    return false;
                }
            }

            return true;
        }

        public static AuthenticationProperties CreateErrorAuthenticationProperties(string error, string errorDescription)
        {
            return new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
            });
        }

        public static IEnumerable<string> GetDestinations(Claim claim)
        {
            switch (claim.Type)
            {
                case Claims.Name or Claims.PreferredUsername:
                    yield return Destinations.AccessToken;

                    if (claim.Subject.HasScope(Scopes.Profile))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Email:
                    yield return Destinations.AccessToken;

                    if (claim.Subject.HasScope(Scopes.Email))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;

                    if (claim.Subject.HasScope(Scopes.Roles))
                        yield return Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }
    }
}
