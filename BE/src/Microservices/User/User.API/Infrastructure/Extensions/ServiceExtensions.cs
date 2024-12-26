using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;

namespace User.API.Infrastructure.Extensions;


public static class ServiceExtensions
{
    public static IServiceCollection RegisterOpenIddict(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddOpenIddict()
            .AddValidation(options =>
            {
                var issuer = configuration["OpenIddict:Issuer"]
                    ?? throw new InvalidOperationException("Issuer not found.");

                var audiesnces = configuration["OpenIddict:Audiences"]
                    ?? throw new InvalidOperationException("Audiences not found.");

                options.SetIssuer(issuer);
                options.AddAudiences(audiesnces);

                var encryptionKey = configuration["OpenIddict:EncryptionKey"]
                    ?? throw new InvalidOperationException("EncryptionKey not found.");
                options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String(encryptionKey)));

                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        services.AddAuthorization();

        return services;
    }
}
