using Auth.Server.Data;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Auth.Server;

// TO DO: Create an admin panel
public class Worker(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken)
            .ConfigureAwait(false);

        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        await AddScopeAsync(scopeManager, "products_api_scope", "Products API scope", "products_api", cancellationToken);
        await AddScopeAsync(scopeManager, "user_api_scope", "User API scope", "user_api", cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var gatewayClient = await manager.FindByClientIdAsync("gateway", cancellationToken)
            .ConfigureAwait(false);

        if (gatewayClient != null)
        {
            await manager.DeleteAsync(gatewayClient, cancellationToken)
                .ConfigureAwait(false);
        }

        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "gateway",
            ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "GraphQl gateway",
            RedirectUris =
                {
                    new Uri("https://localhost:8081/graphql/")
                },
            PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:8081/graphql/")
                },
            Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "products_api_scope",
                    Permissions.Prefixes.Scope + "user_api_scope"
                },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        }, cancellationToken).ConfigureAwait(false);

        var client = await manager.FindByClientIdAsync("nextjs_client", cancellationToken)
            .ConfigureAwait(false);

        if (client != null)
        {
            await manager.DeleteAsync(client, cancellationToken)
                .ConfigureAwait(false);
        }

        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "nextjs_client",
            ClientSecret = "CBDA4D22-87F3-44B5-A5E3-6FE1E111FBFB",
            ConsentType = ConsentTypes.Explicit,
            DisplayName = "NextJs Client Application",
            RedirectUris =
                {
                    new Uri("https://localhost:3000/api/auth/callback/authServer")
                },
            PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:3000/api/auth/callback/authServer")
                },
            Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "products_api_scope",
                    Permissions.Prefixes.Scope + "user_api_scope"
                },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        }, cancellationToken).ConfigureAwait(false);

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task AddScopeAsync(IOpenIddictScopeManager scopeManager, string scopeName, string displayName, string resoures, CancellationToken cancellationToken)
    {
        var apiScope = await scopeManager.FindByNameAsync(scopeName, cancellationToken)
            .ConfigureAwait(false);

        if (apiScope != null)
        {
            await scopeManager.DeleteAsync(apiScope, cancellationToken)
                .ConfigureAwait(false);
        }

        await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
        {
            DisplayName = displayName,
            Name = scopeName,
            Resources =
            {
               resoures
            }
        }, cancellationToken).ConfigureAwait(false);
    }
}
