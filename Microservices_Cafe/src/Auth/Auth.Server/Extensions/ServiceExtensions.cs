﻿using Auth.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Security.Cryptography.X509Certificates;
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

    public static IServiceCollection RegisterOpenIddict(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
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

                        if (environment.IsDevelopment())
                        {
                            options.AddDevelopmentSigningCertificate();
                        }
                        else
                        {
                            var certificates = configuration.GetSection("OpenIddictCertificates").Get<IdentityCertificates>()
                                    ?? throw new InvalidOperationException("Certificates not found.");
                            options.AddRequiredCertificates(certificates);
                        }

                        options.UseAspNetCore()
                               .EnableAuthorizationEndpointPassthrough()
                               .EnableLogoutEndpointPassthrough()
                               .EnableTokenEndpointPassthrough()
                               .EnableUserinfoEndpointPassthrough()
                               .EnableStatusCodePagesIntegration();
                    });
        return services;
    }

    private static OpenIddictServerBuilder AddRequiredCertificates(
        this OpenIddictServerBuilder builder,
        IdentityCertificates certificates)
    {
        var signingCertBytes =
            File.ReadAllBytes($"{certificates.Path}/{certificates.SigningCertificate.Thumbprint}{certificates.CertExtension}");
        builder.AddSigningCertificate(new X509Certificate2(signingCertBytes, certificates.Password));

        return builder;
    }
}

