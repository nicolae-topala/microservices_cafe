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
        var apiScope = await scopeManager.FindByNameAsync("products_api_scope", cancellationToken)
            .ConfigureAwait(false);

        if (apiScope != null)
        {
            await scopeManager.DeleteAsync(apiScope, cancellationToken)
                .ConfigureAwait(false);
        }

        await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
        {
            DisplayName = "Products API scope",
            Name = "products_api_scope",
            Resources =
            {
               "products_api"
            }
        }, cancellationToken).ConfigureAwait(false);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var client = await manager.FindByClientIdAsync("gateway", cancellationToken)
            .ConfigureAwait(false);

        if (client != null)
        {
            await manager.DeleteAsync(client, cancellationToken)
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
                    Permissions.Prefixes.Scope + "products_api_scope"
                },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        }, cancellationToken).ConfigureAwait(false);

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
