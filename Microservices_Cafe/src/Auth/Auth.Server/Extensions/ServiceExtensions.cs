using Auth.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants.Permissions;

namespace Auth.Server.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseOpenIddict();
        });
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

    public static IServiceCollection AddQuartzForOpenIddict(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }

    public static IServiceCollection RegisterOpenIddict(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenIddict()
                    .AddCore(options =>
                    {
                        options.UseEntityFrameworkCore()
                               .UseDbContext<ApplicationDbContext>();

                        options.UseQuartz();
                    })
                    .AddServer(options =>
                    {
                        options.SetAuthorizationEndpointUris("connect/authorize")
                               .SetLogoutEndpointUris("connect/logout")
                               .SetTokenEndpointUris("connect/token")
                               .SetUserinfoEndpointUris("connect/userinfo");

                        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                        options.AllowAuthorizationCodeFlow();

                        var encryptionKey = configuration.GetValue<string>("EncryptionKey")
                            ?? throw new InvalidOperationException("Encryption Key not found.");
                        options.AddEncryptionKey(new SymmetricSecurityKey(
                            Convert.FromBase64String(encryptionKey)));

                        options.AddDevelopmentEncryptionCertificate()
                               .AddDevelopmentSigningCertificate();

                        options.UseAspNetCore()
                               .EnableAuthorizationEndpointPassthrough()
                               .EnableLogoutEndpointPassthrough()
                               .EnableTokenEndpointPassthrough()
                               .EnableUserinfoEndpointPassthrough()
                               .EnableStatusCodePagesIntegration();
                    })
                    .AddValidation(options =>
                    {
                        options.UseLocalServer();
                        options.UseAspNetCore();
                    });
        return services;

    }
}

